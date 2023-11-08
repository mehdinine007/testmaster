

using CompanyManagement.Application.CompanyService;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace OrderManagement.Application.CompanyManagement.GrpcServer
{
    public class GrpcOrderService : CompanyServiceGrpc.CompanyServiceGrpcBase
    {
        private readonly IOrderDeliveryService _orderDeliveryService;
        public GrpcOrderService(IOrderDeliveryService orderDeliveryService)
        {
            _orderDeliveryService = orderDeliveryService;
        }
        public override async Task<ClientOrderDetailResponse> CheckOrderDeliveryDate(ClientOrderDetailRequest request, ServerCallContext context)
        {
            //var clientsOrderDeliveryDateValidation =await _baseInformationSevice.CheckOrderDeliveryDate(request.NationalCode, request.OrderId);
            //if (clientsOrderDeliveryDateValidation)
            //{
          
            var orderDelay = await _orderDeliveryService.GetOrderDelivary(request.NationalCode, request.OrderId);
            if(orderDelay == null)
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
                CarDesc = orderDelay.CarDesc
            });
            return ClientOrderDetail;
            //}
            //return null;
        }

    }
}

