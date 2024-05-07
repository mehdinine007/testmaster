using Volo.Abp.Application.Services;
using CompanyManagement.Application.Contracts.Services;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using CompanyManagement.Application.OrderService;
using CompanyManagement.Application.Contracts.Dto;

namespace CompanyManagement.Application.Implementations;

public class OrderGrpcClientService : ApplicationService, IOrderGrpcClientService
{
    private readonly IConfiguration _configuration;

    public OrderGrpcClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GetOrderByIdResponseDto> GetOrderById(int orderId)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        var httpHandler = new HttpClientHandler();
        httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:OrderUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
        var client = new OrderGrpcService.OrderGrpcServiceClient(channel);
        var result = await client.GetOrderByIdAsync(new GetOrderByIdRequest()
        {
            OrderId = orderId
        });

        return new(result.ProductId,
            result.ProductCode,
            result.OrganizationId,
            result.OrderStatus);
    }
}
