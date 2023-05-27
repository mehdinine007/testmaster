using Esale.Core.CrossCuttingConcerns.Caching.Redis;
using Esale.Core.Utility.Results;
using Google.Protobuf;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _configuration { get; set; }
        private readonly RedisCacheManager _redisCacheManager;  
        public CapacityControlAppService(IRepository<SaleDetail, int> saleDetailRepository, IConfiguration configuration)
        {
            _saleDetailRepository = saleDetailRepository;
            _configuration = configuration;
            _redisCacheManager = new RedisCacheManager("RedisCache:ConnectionString");
        }
        private List<SaleDetail> GetSaleDetails()
        {
            var currentTime = DateTime.Now;
            return _saleDetailRepository
                .WithDetails()
                .Where(x => x.SalePlanStartDate <= currentTime && currentTime <= x.SalePlanEndDate && x.Visible)
                .ToList();
        }
        public async Task<IResult> SaleDetail()
        {
            var saledetail = GetSaleDetails();
            if (saledetail != null && saledetail.Count > 0)
            {
                foreach (var row in saledetail)
                {
                    string _key = string.Format(CapacityControlConstants.SaleDetailPrefix, row.UID.ToString());
                    try
                    {
                        await _redisCacheManager.AddAsync<string>(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), row.SaleTypeCapacity.ToString());
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
            var saledetail = GetSaleDetails();
            if (saledetail != null && saledetail.Count > 0)
            {
                foreach (var row in saledetail)
                {
                    string _key = string.Format(CapacityControlConstants.PaymentCountPrefix, row.UID.ToString());
                    int _value = 0;
                    using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
                    {
                        var productAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                        var productDtos = await productAppService.GetPaymentStatusList(new PaymentStatusDto()
                        {
                            RelationId = row.Id
                        });
                        if (productDtos != null && productDtos.Any(x => x.Status == 0))
                        {
                            _value = productDtos.FirstOrDefault(x => x.Status == 0).Count;
                        }
                    }
                    await _redisCacheManager.AddAsync<string>(string.Format(CapacityControlConstants.CapacityControlPrefix, _key), _value.ToString());
                }
            }
            return new SuccsessResult();
        }


        public async Task GrpcPaymentTest()
        {
            using (var channel = GrpcChannel.ForAddress(_configuration.GetSection("gRPC:PaymentUrl").Value))
            {
                var productAppService = channel.CreateGrpcService<IGrpcPaymentAppService>();
                var productDtos = await productAppService.GetPaymentStatusList(new PaymentStatusDto() { RelationId = 0 });
            }

        }

    }
}
