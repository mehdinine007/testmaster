using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Dtos.Grpc.Client.Sign;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Application.SignGrpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class SignGrpcClient : ApplicationService,ISignGrpcClient
    {
        private readonly IConfiguration _configuration;
        public SignGrpcClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private SignGrpc.SignGrpcService.SignGrpcServiceClient SignGrpcServiceClient()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Gateway:GrpcAddress"), new GrpcChannelOptions { HttpHandler = httpHandler });
            return new SignGrpc.SignGrpcService.SignGrpcServiceClient(channel);
        }

        public async Task<CreateSignGrpcClientResponse> CreateSign(CreateSignGrpcClientRequest createSignGrpcClientRequest)
        {
            var createSign = await SignGrpcServiceClient().CreateSignAsync(JsonConvert.DeserializeObject<CreateSignGrpcInput>(JsonConvert.SerializeObject(createSignGrpcClientRequest)));
            var _ret = JsonConvert.DeserializeObject<CreateSignGrpcClientResponse>(JsonConvert.SerializeObject(createSign));
            return await Task.FromResult(_ret);

        }

        public async Task<InquiryGrpcClientResponse> InquirySign(Guid workflowTicket)
        {
            var inquirySign = SignGrpcServiceClient().InquirySign(new InquirySignInput { WorkflowTicket= workflowTicket.ToString() });
            var _ret = JsonConvert.DeserializeObject<InquiryGrpcClientResponse>(JsonConvert.SerializeObject(inquirySign));
            return await Task.FromResult(_ret);
        }

    }
}
