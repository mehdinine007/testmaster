using Esale.GetwayServiceGrpc;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Services;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.UserManagement.Implementations
{
    public class GetwayGrpcClient : ApplicationService , IGetwayGrpcClient
    {
        private IConfiguration _configuration { get; set; }
        public GetwayGrpcClient(IConfiguration configuration)
        {
                _configuration = configuration;
        }

        private GetwayServiceGrpc.GetwayServiceGrpcClient PostCaptchaServiceGrpcClient()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GatewayManagement:GrpcAddress"));
            return new GetwayServiceGrpc.GetwayServiceGrpcClient(channel);
        }



        public async Task<Domain.UserManagement.CommonService.Dto.Getway.HttpResponseMessageDto> GetCaptcha(Domain.UserManagement.CommonService.Dto.Getway.ContentInputDto Content)
        {
            try
            {
                //ContentInputDto ContentInput1 = new()
                //{
                //    ContentValue = Content.ContentValue,
                //    KeyValue = Content.KeyValue
                //};
                var getCatcha = PostCaptchaServiceGrpcClient().GetCaptcha(new()
                {
                    ContentValue = Content.ContentValue,
                    KeyValue = Content.KeyValue
                });
                return await Task.FromResult(new Domain.UserManagement.CommonService.Dto.Getway.HttpResponseMessageDto
                {
                    IsSuccessStatusCode = getCatcha.IsSuccessStatusCode,
                    StringContent = getCatcha.StringContent
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Domain.UserManagement.CommonService.Dto.Getway.SendBoxServiceDto> SendService(Domain.UserManagement.CommonService.Dto.Getway.SendBoxServiceInput input)
        {
            
                var sendSeivice = PostCaptchaServiceGrpcClient().SendService(new()
                {
                    Recipient = input.Recipient,
                    Text = input.Text,
                    Provider = (Esale.GetwayServiceGrpc.ProviderSmsTypeEnum)input.Provider,
                    Type = (Esale.GetwayServiceGrpc.TypeMessageEnum)input.Type
                });
                return await Task.FromResult(new Domain.UserManagement.CommonService.Dto.Getway.SendBoxServiceDto
                {
                    Success = sendSeivice.Success,
                    DataResult = sendSeivice.DataResult,
                    Message = sendSeivice.Message,
                    MessageCode = sendSeivice.MessageCode
                });
         
        }
    
    }
}
