

using CompanyManagement.Application.CompanyManagement.Implementations;
using CompanyManagement.Application.CompanyService;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Application.CompanyManagement.GrpcServer
{
    public class GrpcCompanyService : CompanyServiceGrpc.CompanyServiceGrpcBase
    {
        private readonly IClientOrderDetailCompanyService _ClientOrderDetailCompanyService;
        public GrpcCompanyService(IClientOrderDetailCompanyService clientOrderDetailCompanyService)
        {
            _ClientOrderDetailCompanyService = clientOrderDetailCompanyService;
        }
        public override async Task<ClientOrderDetailResponse> CheckOrderDeliveryDate(ClientOrderDetailRequest request, ServerCallContext context)
        {
            //var clientsOrderDeliveryDateValidation =await _baseInformationSevice.CheckOrderDeliveryDate(request.NationalCode, request.OrderId);
            //if (clientsOrderDeliveryDateValidation)
            //{

            var orderDelay = await _ClientOrderDetailCompanyService.GetOrderDelivary(request.NationalCode, request.OrderId);
            if (orderDelay == null)
            {
                return new ClientOrderDetailResponse();
            }
            var ClientOrderDetail = await Task.FromResult(new ClientOrderDetailResponse()
            {
                NationalCode = orderDelay.NationalCode,
                TranDate = orderDelay.TranDate.HasValue ? Timestamp.FromDateTimeOffset(orderDelay.TranDate.Value) : new(),
                PayedPrice = orderDelay.PayedPrice,
                ContRowId = orderDelay.ContRowId,
                Vin = orderDelay.Vin,
                DeliveryDate = orderDelay.DeliveryDate.HasValue ? Timestamp.FromDateTimeOffset(orderDelay.DeliveryDate.Value) : new(),
                BodyNumber = orderDelay.BodyNumber,
                FinalPrice = orderDelay.FinalPrice,
                CarDesc = orderDelay.CarDesc,
                ContRowIdDate = orderDelay.ContRowIdDate.HasValue ? Timestamp.FromDateTimeOffset(orderDelay.DeliveryDate.Value) : new(),
            });
            return ClientOrderDetail;
            //}
            //return null;
        }

        public async override Task<ClientOrderDetailListResponse> GetOrderDetailList(ClientOrderDetailListRequest request, ServerCallContext context)
        {
            var clientOrderDetails = await _ClientOrderDetailCompanyService.GetList(request.NationalCode);
            var clientOrderDetailResponse = new ClientOrderDetailListResponse();
            clientOrderDetails.ForEach(x =>
            {
                var clientOrderDetail = new ClientOrderDetail
                {
                    SaleType = x.SaleType.Value,
                    CarCode = x.CarCode ?? "",
                    CarDesc = x.CarDesc ?? "",
                    ModelType = x.ModelType.Value,
                    PaypaidPrice ={x.PaypaidPrice.Select(x=> new PaypaidPrice
                     {
                       PayedPrice=x.PayedPrice,
                       TranDate=Timestamp.FromDateTime(x.TranDate.ToUniversalTime()),

                     })},
                    TurnDate = {x.TurnDate.Select(x=>new TurnDate
                    {
                         EndTurnDate=Timestamp.FromDateTime(x.EndTurnDate.ToUniversalTime()),
                         StartTurnDate=Timestamp.FromDateTime(x.StartTurnDate.ToUniversalTime()),
                    })}
                };
                if (x.DeliveryDate.HasValue)
                {
                    clientOrderDetail.DeliveryDate= Timestamp.FromDateTime(x.DeliveryDate.Value.ToUniversalTime());
                }
                if (x.FactorDate.HasValue)
                {
                    clientOrderDetail.FactorDate = Timestamp.FromDateTime(x.FactorDate.Value.ToUniversalTime());
                }
                if (x.IntroductionDate.HasValue)
                {
                    clientOrderDetail.IntroductionDate = Timestamp.FromDateTime(x.IntroductionDate.Value.ToUniversalTime());
                }
                clientOrderDetailResponse.ClientOrderDetail.Add(clientOrderDetail);
            });
            return clientOrderDetailResponse;
        }
    }
}

