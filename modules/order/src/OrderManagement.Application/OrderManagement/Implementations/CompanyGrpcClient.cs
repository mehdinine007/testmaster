using OrderManagement.Application.Contracts;
using Volo.Abp.Application.Services;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.CompanyService;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class CompanyGrpcClient: ApplicationService,ICompanyGrpcClient 
    {
        private readonly IConfiguration _configuration;
        public CompanyGrpcClient(IConfiguration configuration)
        {
            _configuration = configuration;
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
                return new ClientOrderDeliveryInformationDto
                {
                    NationalCode = deliverDateValidation.NationalCode,
                    TranDate = deliverDateValidation.TranDate.ToDateTime(),// ? Timestamp.FromDateTime(deliverDateValidation.TranDate) : new,
                    PayedPrice = deliverDateValidation.PayedPrice,
                    ContRowId = deliverDateValidation.ContRowId,
                    Vin = deliverDateValidation.Vin,
                    BodyNumber = deliverDateValidation.BodyNumber,
                    DeliveryDate = deliverDateValidation.DeliveryDate.ToDateTime(),
                    FinalPrice = deliverDateValidation.FinalPrice,
                    CarDesc = deliverDateValidation.CarDesc
                };
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
