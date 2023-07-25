﻿using Esale.Core.Caching.Redis;
using Esale.Core.Utility.Results;
using Grpc.Net.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using Esale.Core.Caching;

namespace OrderManagement.Application.OrderManagement;

public class CapacityControlAppService : ApplicationService, ICapacityControlAppService
{
    private readonly ISaleDetailService _saleDetailService;
    private readonly IAgencySaleDetailService _agencySaleDetailService;
    private readonly IEsaleGrpcClient _grpcClient;
    private readonly ICommonAppService _commonAppService;
    private readonly ICacheManager _cacheManager;
    private IConfiguration _configuration { get; set; }
    public CapacityControlAppService(IConfiguration configuration, IEsaleGrpcClient grpcClient, IAgencySaleDetailService agencySaleDetailService, ISaleDetailService saleDetailService, ICommonAppService commonAppService, ICacheManager cacheManager)
    {
        _configuration = configuration;
        _grpcClient = grpcClient;
        _agencySaleDetailService = agencySaleDetailService;
        _saleDetailService = saleDetailService;
        _commonAppService = commonAppService;
        _cacheManager = cacheManager;
    }
    public async Task<IResult> SaleDetail()
    {
        var saledetails = _saleDetailService.GetActiveList();
        if (saledetails != null && saledetails.Count > 0)
        {
            foreach (var saledetail in saledetails)
            {
                string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saledetail.UID.ToString());
                try
                {
                    await _cacheManager.SetStringAsync(_key,
                        CapacityControlConstants.CapacityControlPrefix,
                        saledetail.SaleTypeCapacity.ToString(),
                        new CacheOptions() 
                        {
                            Provider = CacheProviderEnum.Redis,
                            RedisHash = false
                        });
                }
                catch (Exception ex)
                {
                    return new ErrorResult(ex.Message, "100");
                }
            }
        }
        return new SuccsessResult();
    }

    public async Task<IResult> Payment()
    {
        var saledetails = _saleDetailService.GetActiveList();
        if (saledetails != null && saledetails.Count > 0)
        {
            foreach (var saledetail in saledetails)
            {
                string _key = string.Format(CapacityControlConstants.PaymentCountPrefix, saledetail.UID.ToString());
                long _value = 0;
                var paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
                {
                    RelationId = saledetail.Id
                });
                if (paymentDtos != null && paymentDtos.Any(x => x.Status == 2))
                {
                    _value = paymentDtos.FirstOrDefault(x => x.Status == 2).Count;
                }
                await _cacheManager.SetStringAsync(_key,
                    CapacityControlConstants.CapacityControlPrefix,
                     _value.ToString(),
                    new CacheOptions()
                    {
                        Provider = CacheProviderEnum.Redis,
                        RedisHash = false
                    });
            }
        }
        return new SuccsessResult();
    }

    public async Task GrpcPaymentTest()
    {
        //var redis = await _redisCacheManager.ScanKeysAsync("n:CapacityControl:*");
        //var redis = _redisCacheManager.RemoveAllAsync("n:CapacityControl:*");
        //var payment = await _grpcClient.RetryForVerify();
        //var _result = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
        //{
        //    RelationId = 60
        //});
        //var handshake = await _grpcClient.HandShake(new PaymentHandShakeDto()
        //{
        //    Amount = 10000,
        //    PspAccountId = 4,
        //    CallBackUrl = "http",
        //    Mobile = "09140352778",
        //    NationalCode = "1092271600",
        //    AdditionalData = "asda"
        //});
        //Validation(165, 1029);
    }

    public async Task<bool> ValidationBySaleDetailUId(Guid saleDetailUId)
    {
        await _commonAppService.ValidateOrderStep(OrderStepEnum.SubmitOrder);
        long _capacity = 0;
        string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saleDetailUId.ToString());
        long.TryParse(await _cacheManager.GetStringAsync(_key, CapacityControlConstants.CapacityControlPrefix,new CacheOptions() { Provider = CacheProviderEnum.Redis,RedisHash = false}), out _capacity);

        _key = string.Format(CapacityControlConstants.PaymentCountPrefix, saleDetailUId.ToString());
        long _request = await _cacheManager.StringIncrementAsync(CapacityControlConstants.CapacityControlPrefix+ _key);

        if (_request > _capacity && _capacity > 0)
        {
            throw new UserFriendlyException(OrderConstant.NoCapacityCreateTicket, code: OrderConstant.NoCapacityCreateTicketId);
        }
        await _commonAppService.SetOrderStep(OrderStepEnum.SubmitOrder);
        return true;
    }

    public async Task<IResult> AgencyValidation(int saledetailid, int? agencyId, List<PaymentStatusModel> paymentDtos)
    {
        var saledetail = _saleDetailService.GetById(saledetailid);
        var agencySaledetail = await _agencySaleDetailService.GetBySaleDetailId(saledetail.Id, agencyId ?? 0);
        if (agencySaledetail == null)
        {
            return new ErrorResult("خطا در بازیابی نمایندگی ها", OrderConstant.NoCapacityCreateTicketId);
        }
        long _agancyCapacity = agencySaledetail.DistributionCapacity;
        int _agencyReserveCount = agencySaledetail.ReserveCount;
        long _agancyPaymentCount = 0;
        var paymentDtosForAgency = paymentDtos.Where(x => x.F2 == agencyId).ToList();
        if (paymentDtosForAgency != null && paymentDtosForAgency.Any(x => x.Status == 2))
        {
            _agancyPaymentCount = paymentDtosForAgency.Where(x => x.Status == 2).Sum(x => x.Count);
        }
        if (_agancyPaymentCount >= _agancyCapacity && _agancyCapacity > 0) //control zarfiat kili
        {
            return new ErrorResult(OrderConstant.AgancyNoCapacityCreateTicket, OrderConstant.AgancyNoCapacityCreateTicketId);
        }
        if (agencySaledetail.ReserveCount > 0)
        {

            if (_agancyPaymentCount < _agencyReserveCount)//agar be reserve nareside
            {
                return new SuccsessResult();
            }
            else
            {
                var _allAgenecyForSaleDetail = await _agencySaleDetailService.GetAgeneciesBySaleDetail(saledetail.Id);
                int _sumReseverCount = _allAgenecyForSaleDetail.Sum(x => x.ReserveCount);//sum reserve
                int FreeSpace = saledetail.SaleTypeCapacity - _sumReseverCount;
                var lsSum = from ag in _allAgenecyForSaleDetail
                            join pr in paymentDtos
                            on ag.AgencyId equals pr.F2
                            where ag.ReserveCount < pr.Count
                            && pr.Status == 2
                            select new
                            {
                                cnt = pr.Count - ag.ReserveCount
                            };
                long _sumBuyMoreThanReserve = lsSum.Sum(x => x.cnt);
                if (_sumBuyMoreThanReserve > FreeSpace) //agar zafiat azad(dovom) tamom shode
                {
                    return new ErrorResult(OrderConstant.AgancyNoCapacityCreateTicket, OrderConstant.AgancyNoCapacityCreateTicketId);
                }
            }
        }
        return new SuccsessResult();
    }
    public async Task<IDataResult<List<PaymentStatusModel>>> Validation(int saleDetaild, int? agencyId)
    {
        var saledetail = _saleDetailService.GetById(saleDetaild);
        if (saledetail == null)
        {
            return new ErrorDataResult<List<PaymentStatusModel>>("خطا در بازیابی برنامه های فروش", OrderConstant.NoCapacityCreateTicketId);
        }
        long _saledetailCapacity = saledetail.SaleTypeCapacity;
        long _saleDetailPaymentCount = 0;
        var paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
        {
            RelationId = saleDetaild,
            IsRelationIdGroup = true,
            IsRelationIdBGroup = true,
        });
        if (paymentDtos != null && paymentDtos.Any(x => x.Status == 2))
        {
            _saleDetailPaymentCount = paymentDtos.Where(x => x.Status == 2).Sum(x => x.Count);
        }
        if (_saleDetailPaymentCount >= _saledetailCapacity && _saledetailCapacity > 0) //control zarfiat koli
        {
            return new ErrorDataResult<List<PaymentStatusModel>>(OrderConstant.NoCapacityCreateTicket, OrderConstant.NoCapacityCreateTicketId);
        }
        if (agencyId != null && agencyId != 0)
        {
            var agency = await AgencyValidation(saledetail.Id, agencyId, paymentDtos);
            if (!agency.Succsess)
                return new ErrorDataResult<List<PaymentStatusModel>>(agency.Message, agency.MessageId);
        }
        return new SuccsessDataResult<List<PaymentStatusModel>>(paymentDtos);
    }
}
