using Esale.Core.Caching.Redis;
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
using Esale.Core.CrossCuttingConcerns.Caching.Redis;

namespace OrderManagement.Application.OrderManagement
{
    public class CapacityControlAppService : ApplicationService, ICapacityControlAppService
    {
        private readonly IRepository<SaleDetail, int> _saleDetailRepository;
        private readonly IRepository<AgencySaleDetail, int> _agencySaleDetail;
        private readonly IEsaleGrpcClient _grpcClient;
        private IConfiguration _configuration { get; set; }
        private readonly RedisCacheManager _redisCacheManager;
        public CapacityControlAppService(IRepository<SaleDetail, int> saleDetailRepository, IConfiguration configuration, IRepository<AgencySaleDetail, int> agencySaleDetail, IEsaleGrpcClient grpcClient)
        {
            _saleDetailRepository = saleDetailRepository;
            _configuration = configuration;
            _redisCacheManager = new RedisCacheManager("RedisCache:ConnectionString");
            _agencySaleDetail = agencySaleDetail;
            _grpcClient = grpcClient;
        }
        private List<SaleDetail> GetSaleDetails()
        {
            var currentTime = DateTime.Now;
            return _saleDetailRepository
                .WithDetails()
                .AsNoTracking()
                .Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible)
                .ToList();
        }
        private SaleDetail GetSaleDetailById(int id)
        {
            var currentTime = DateTime.Now;
            return _saleDetailRepository
                .WithDetails()
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        private AgencySaleDetail GetAgancySaleDetail(int saleDetailId, int agancyId)
        {
            return _agencySaleDetail
                .WithDetails()
                .AsNoTracking()
                .FirstOrDefault(x => x.SaleDetailId == saleDetailId && !x.IsDeleted && x.AgencyId == agancyId);
        }
        private long GetReservCount(int saleDetailId)
        {
            return _agencySaleDetail
                .WithDetails()
                .AsNoTracking()
                .Where(x => x.SaleDetailId == saleDetailId)
                .Sum(x => x.ReserveCount);
        }
        public async Task<IResult> SaleDetail()
        {
            var saledetails = GetSaleDetails();
            if (saledetails != null && saledetails.Count > 0)
            {
                foreach (var saledetail in saledetails)
                {
                    string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saledetail.UID.ToString());
                    try
                    {
                        await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), saledetail.SaleTypeCapacity.ToString());
                        //var agences = GetAgancySaleDetails(saledetail.Id);
                        //if (agences != null && agences.Count > 0)
                        //{
                        //    foreach (var agency in agences)
                        //    {
                        //        _key = string.Format(CapacityControlConstants.AgancySaleDetailPrefix, saledetail.UID.ToString(), agency.AgencyId.ToString());
                        //        await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), agency.DistributionCapacity.ToString());
                        //    }
                        //}
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
            var saledetails = GetSaleDetails();
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
                    if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
                    {
                        _value = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
                    }
                    //using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
                    //{
                    //    var paymentAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                    //    var paymentDtos = paymentAppService.GetPaymentStatusList(new PaymentStatusDto()
                    //    {
                    //        RelationId = saledetail.Id
                    //    });
                    //    if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
                    //    {
                    //        _value = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
                    //    }
                    //}
                    await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), _value.ToString());
                    //var agences = GetAgancySaleDetails(saledetail.Id);
                    //if (agences != null && agences.Count > 0)
                    //{
                    //    foreach (var agency in agences)
                    //    {
                    //        _key = string.Format(CapacityControlConstants.AgancyPaymentPrefix, saledetail.UID.ToString(), agency.AgencyId.ToString());
                    //        await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), "0");
                    //    }
                    //}
                }
            }
            return new SuccsessResult();
        }

        public async Task GrpcPaymentTest()
        {
            var redis = _redisCacheManager.RemoveAllAsync("n:CapacityControl:*");
            var payment = await _grpcClient.RetryForVerify();
            var _result = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
            {
                RelationId = 60
            });
        }

        public async Task<bool> ValidationBySaleDetailUId(Guid saleDetailUId)
        {
            long _capacity = 0;
            string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, saleDetailUId.ToString());
            long.TryParse(await _redisCacheManager.GetStringAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key)), out _capacity);

            _key = string.Format(CapacityControlConstants.PaymentCountPrefix, saleDetailUId.ToString());
            long _request = await _redisCacheManager.StringIncrementAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key));

            //_key = string.Format(CapacityControlConstants.PaymentRequestPrefix, saleDetailId.ToString());
            //await _redisCacheManager.StringIncrementAsync(string.Format(CapacityControlConstants.CapacityControlPrefix));
            if (_request > _capacity && _capacity > 0)
            {
                throw new UserFriendlyException(CapacityControlConstants.NoCapacityCreateTicket,code: CapacityControlConstants.NoCapacityCreateTicketId);
            }
            //agancy
            //if (agancyId != null && agancyId != 0)
            //{
            //    long _agancyCapacity = 0;
            //    _key = string.Format(CapacityControlConstants.AgancySaleDetailPrefix, saleDetailUId.ToString(), agancyId.ToString());
            //    long.TryParse(await _redisCacheManager.GetStringAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key)), out _agancyCapacity);

            //    _key = string.Format(CapacityControlConstants.AgancyPaymentPrefix, saleDetailUId.ToString(), agancyId.ToString());
            //    long _agancyRequest = await _redisCacheManager.StringIncrementAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key));
            //    if (_agancyRequest > _agancyCapacity && _agancyCapacity > 0)
            //    {
            //        return new ErrorResult(CapacityControlConstants.AgancyNoCapacityCreateTicket, CapacityControlConstants.AgancyNoCapacityCreateTicketId);
            //    }
            //}
            return true;
        }

        public async Task<IResult> Validation(int saleDetaild, int? agencyId)
        {
            var saledetail = GetSaleDetailById(saleDetaild);
            if (saledetail == null)
            {
                throw new UserFriendlyException("خطا در بازیابی برنامه های فروش");
            }
            long _saledetailCapacity = saledetail.SaleTypeCapacity;
            long _saleDetailPaymentCount = 0;
            var paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
            {
                RelationId = saleDetaild
            });
            if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
            {
                _saleDetailPaymentCount = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
            }

            //using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
            //{
            //    var paymentAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
            //    var paymentDtos = paymentAppService.GetPaymentStatusList(new PaymentStatusDto()
            //    {
            //        RelationIdB = saleDetaild
            //    });
            //    if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
            //    {
            //        _saleDetailPaymentCount = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
            //    }
            //}
            if (_saleDetailPaymentCount > _saledetailCapacity && _saledetailCapacity > 0)
            {
                return new ErrorResult(CapacityControlConstants.NoCapacityCreateTicket, CapacityControlConstants.NoCapacityCreateTicketId);
            }
            if (agencyId != null && agencyId != 0)
            {
                var agencySaledetail = GetAgancySaleDetail(saleDetaild, agencyId??0);
                if(agencySaledetail == null)
                {
                    throw new UserFriendlyException("خطا در بازیابی نمایندگی ها");
                }
                long _agancyCapacity = agencySaledetail.DistributionCapacity;
                long _agancyPaymentCount = 0;
                paymentDtos = await _grpcClient.GetPaymentStatusList(new PaymentStatusDto()
                {
                    RelationIdB = saleDetaild,
                    RelationIdC = agencyId??0
                });
                if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
                {
                    _agancyPaymentCount = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
                }
                if (_agancyCapacity > _agancyPaymentCount && _agancyPaymentCount > 0)
                {
                    return new ErrorResult(CapacityControlConstants.AgancyNoCapacityCreateTicket, CapacityControlConstants.AgancyNoCapacityCreateTicketId);
                }
                //using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
                //{
                //    var paymentAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                //    var paymentDtos = paymentAppService.GetPaymentStatusList(new PaymentStatusDto()
                //    {
                //        RelationIdB = saleDetaild,
                //        RelationIdC = agencyId
                //    });
                //    if (paymentDtos != null && paymentDtos.Any(x => x.Status == 0))
                //    {
                //        _agancyPaymentCount = paymentDtos.FirstOrDefault(x => x.Status == 0).Count;
                //    }
                //    if (_agancyCapacity > _agancyPaymentCount && _agancyPaymentCount > 0)
                //    {
                //        return new ErrorResult(CapacityControlConstants.AgancyNoCapacityCreateTicket, CapacityControlConstants.AgancyNoCapacityCreateTicketId);
                //    }

                //}
                if (agencySaledetail.ReserveCount > 0)
                {
                    long freeCapacity = (_saledetailCapacity) - GetReservCount(saleDetaild) - (_saleDetailPaymentCount);
                    if (freeCapacity > 0)
                    {
                        return new SuccsessResult();
                    }
                    if (agencySaledetail.ReserveCount< _agancyPaymentCount)
                    {
                        return new ErrorResult(CapacityControlConstants.AgancyNoCapacityCreateTicket, CapacityControlConstants.AgancyNoCapacityCreateTicketId);
                    }
                }
            }
            return new SuccsessResult();
        }
    }
}
