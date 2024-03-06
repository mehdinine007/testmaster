using OrderManagement.Application.Contracts;
using Volo.Abp.Application.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client;
using System.Linq;
using System.Collections.Generic;
using IFG.Core.Caching;
using Newtonsoft.Json;
using OrderManagement.Application.CompanyService;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class CompanyGrpcClient : ApplicationService, ICompanyGrpcClient
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheManager _cacheManager;
        public CompanyGrpcClient(IConfiguration configuration, ICacheManager cacheManager)
        {
            _configuration = configuration;
            _cacheManager = cacheManager;
        }

        public async Task<List<ClientOrderDetailDto>> GetOrderDetailList(string nationalCode)
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Company:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new CompanyServiceGrpc.CompanyServiceGrpcClient(channel);
            TimeSpan secondsInDay = TimeSpan.FromDays(1);
            var ttl = secondsInDay.TotalSeconds;
            string OrderDetailList = await _cacheManager.GetStringAsync(nationalCode, RedisConstants.ClientOrderDetailPrefix
           , new CacheOptions()
           {
               Provider = CacheProviderEnum.Redis
           });

            if (!string.IsNullOrEmpty(OrderDetailList))
            {
                return JsonConvert.DeserializeObject<List<ClientOrderDetailDto>>(OrderDetailList);
            }
            var result = await client.GetOrderDetailListAsync(new ClientOrderDetailListRequest { NationalCode = nationalCode });
            var orderDetails = result.ClientOrderDetail.Select(x => new ClientOrderDetailDto
            {
                DeliveryDate = x.DeliveryDate?.ToDateTime(),
                FactorDate = x.FactorDate?.ToDateTime(),
                IntroductionDate = x.IntroductionDate?.ToDateTime(),
                SaleType = x.SaleType,
                CarCode = x.CarCode,
                CarDesc = x.CarDesc,
                ModelType = x.ModelType,
                PaypaidPrice = x.PaypaidPrice.Select(x => new PaypaidPriceDto
                {
                    PayedPrice = x.PayedPrice,
                    TranDate = x.TranDate.ToDateTime(),

                }).ToList(),
                TurnDate = x.TurnDate.Select(x => new TurnDateDto
                {
                    EndTurnDate = x.EndTurnDate.ToDateTime(),
                    StartTurnDate = x.StartTurnDate.ToDateTime(),
                }).ToList(),

            }).ToList();
            if (orderDetails.Count>0)
            {
                var json = JsonConvert.SerializeObject(orderDetails);
                await _cacheManager.SetStringAsync(nationalCode, RedisConstants.ClientOrderDetailPrefix, json, new CacheOptions()
                {
                    Provider = CacheProviderEnum.Redis
                }, ttl);
            }
            return orderDetails;
        }
        public async Task<ClientOrderDeliveryInformationDto> ValidateClientOrderDeliveryDate(ClientOrderDeliveryInformationRequestDto clientOrderRequest)
        {
            try
            {
                var httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Company:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
                // var client1 = new UserServiceGrpc.UserServiceGrpcClient(channel);
                var client = new CompanyServiceGrpc.CompanyServiceGrpcClient(channel);

                var deliverDateValidation = await client.CheckOrderDeliveryDateAsync(new CompanyService.ClientOrderDetailRequest
                {
                    NationalCode = clientOrderRequest.NationalCode,
                    OrderId = clientOrderRequest.OrderId
                });
                if (deliverDateValidation == null || string.IsNullOrEmpty(deliverDateValidation.NationalCode))
                {
                    return null;
                }
                return new ClientOrderDeliveryInformationDto
                {
                    NationalCode = deliverDateValidation?.NationalCode,
                    TranDate = deliverDateValidation?.TranDate.ToDateTime(),// ? Timestamp.FromDateTime(deliverDateValidation.TranDate) : new,
                    PayedPrice = deliverDateValidation?.PayedPrice,
                    ContRowId = deliverDateValidation?.ContRowId,
                    Vin = deliverDateValidation?.Vin,
                    ContRowIdDate = deliverDateValidation?.ContRowIdDate.ToDateTime(),
                    //ContRowIdDate = deliverDateValidation?.ContRowId
                    BodyNumber = deliverDateValidation?.BodyNumber,
                    DeliveryDate = deliverDateValidation?.DeliveryDate.ToDateTime(),
                    FinalPrice = deliverDateValidation?.FinalPrice,
                    CarDesc = deliverDateValidation?.CarDesc
                };
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
