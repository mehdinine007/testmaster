using Grpc.Core;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderService;
using System.Threading.Tasks;

namespace OrderManagement.Application.GrpcServer.OrderService;

public class OrderGrpcServiceImplementor : OrderGrpcService.OrderGrpcServiceBase
{
    private readonly IOrderAppService _orderAppService;

    public OrderGrpcServiceImplementor(IOrderAppService orderAppService)
    {
        _orderAppService = orderAppService;
    }
    public override async Task<GetOrderByIdResponse> GetOrderById(GetOrderByIdRequest request, ServerCallContext context)
    {
        var order = await _orderAppService.GetOrderById(request.OrderId);
        return new GetOrderByIdResponse
        {
            OrderStatus = order.OrderStatus,
            OrganizationId = order.OrganizationId,
            ProductCode = order.ProductCode,
            ProductId = order.ProductId
        };
    }
}
