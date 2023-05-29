using Esale.Core.CrossCuttingConcerns.Caching.Redis;
using Esale.Core.Utility.Results;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using OrderManagement.Domain;
using PaymentManagement.Application.Contracts.IServices;
using PaymentManagement.Application.Contracts.PaymentManagement.Dtos;
using PaymentManagement.Application.Contracts.PaymentManagement.IServices;
using ProtoBuf.Grpc.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement
{
    public class CapacityControlAppService : ApplicationService, ICapacityControlAppService
    {
        private readonly IRepository<SaleDetail, int> _saleDetailRepository;
        private readonly IRepository<AgencySaleDetail, int> _agencySaleDetail;
        private IConfiguration _configuration { get; set; }
        private readonly RedisCacheManager _redisCacheManager;
        public CapacityControlAppService(IRepository<SaleDetail, int> saleDetailRepository, IConfiguration configuration, IRepository<AgencySaleDetail, int> agencySaleDetail)
        {
            _saleDetailRepository = saleDetailRepository;
            _configuration = configuration;
            _redisCacheManager = new RedisCacheManager("RedisCache:ConnectionString");
            _agencySaleDetail = agencySaleDetail;
        }
        private List<SaleDetail> GetSaleDetails()
        {
            var currentTime = DateTime.Now;
            return _saleDetailRepository
                .WithDetails()
                .Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible)
                .ToList();
        }
        private List<AgencySaleDetail> GetAgancySaleDetails(int saleDetailId)
        {
            return _agencySaleDetail
                .WithDetails()
                .Where(x => x.SaleDetailId == saleDetailId && !x.IsDeleted)
                .ToList();
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
                        var agences = GetAgancySaleDetails(saledetail.Id);
                        if (agences != null && agences.Count > 0)
                        {
                            foreach (var agency in agences)
                            {
                                _key = string.Format(CapacityControlConstants.AgancySaleDetailPrefix, saledetail.UID.ToString(), agency.AgencyId.ToString());
                                await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), agency.DistributionCapacity.ToString());
                            }
                        }
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
                    int _value = 0;
                    using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
                    {
                        var productAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                        var productDtos = productAppService.GetPaymentStatusList(new PaymentStatusDto()
                        {
                            RelationId = saledetail.Id
                        });
                        if (productDtos != null && productDtos.Any(x => x.Status == 0))
                        {
                            _value = productDtos.FirstOrDefault(x => x.Status == 0).Count;
                        }
                    }
                    await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), _value.ToString());
                    var agences = GetAgancySaleDetails(saledetail.Id);
                    if (agences != null && agences.Count > 0)
                    {
                        foreach (var agency in agences)
                        {
                            _key = string.Format(CapacityControlConstants.AgancyPaymentPrefix, saledetail.UID.ToString(), agency.AgencyId.ToString());
                            await _redisCacheManager.StringSetAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), "0");
                        }
                    }
                }
            }
            return new SuccsessResult();
        }

        public async Task GrpcPaymentTest()
        {
            using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
            {
                var productAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                var productDtos = productAppService.GetPaymentStatusList(new PaymentStatusDto() { RelationId = 0 });
            }


        }

        public async Task<IResult> SaleDetailValidation(Guid saleDetailUId, int? agancyId)
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
                return new ErrorResult(CapacityControlConstants.NoCapacityCreateTicket, CapacityControlConstants.NoCapacityCreateTicketId);
            }
            //agancy
            if (agancyId != null && agancyId != 0)
            {
                long _agancyCapacity = 0;
                _key = string.Format(CapacityControlConstants.AgancySaleDetailPrefix, saleDetailUId.ToString(), agancyId.ToString());
                long.TryParse(await _redisCacheManager.GetStringAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key)), out _agancyCapacity);

                _key = string.Format(CapacityControlConstants.AgancyPaymentPrefix, saleDetailUId.ToString(), agancyId.ToString());
                long _agancyRequest = await _redisCacheManager.StringIncrementAsync(string.Format(CapacityControlConstants.CapacityControlPrefix, _key));
                if (_agancyRequest > _agancyCapacity && _agancyCapacity > 0)
                {
                    return new ErrorResult(CapacityControlConstants.AgancyNoCapacityCreateTicket, CapacityControlConstants.AgancyNoCapacityCreateTicketId);
                }
            }
            return new SuccsessResult();
        }
    }
}
