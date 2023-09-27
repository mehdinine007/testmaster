using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using UserManagement.Application.Contracts.Models.SendBox;
using UserManagement.Application.Contracts.UserManagement.Services;
using Volo.Abp.Application.Services;
using GetwayServiceGrpc = Esale.GetwayServiceGrpc.GetwayServiceGrpc;
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



        public async Task<HttpResponseMessageDto> GetCaptcha(ContentInputDto Content)
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
                return await Task.FromResult(new HttpResponseMessageDto
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

        public async Task<SendBoxServiceDto> SendService(SendBoxServiceInput input)
        {
            try
            {

                var sendSeivice = PostCaptchaServiceGrpcClient().SendService(new()
                {
                    Recipient = input.Recipient,
                    Text = input.Text,
                    Provider = (Esale.GetwayServiceGrpc.ProviderSmsTypeEnum)input.Provider,
                    Type = (Esale.GetwayServiceGrpc.TypeMessageEnum)input.Type
                });
                return await Task.FromResult(new SendBoxServiceDto
                {
                    Success = sendSeivice.Success,
                    DataResult = sendSeivice.DataResult,
                    Message = sendSeivice.Message,
                    MessageCode = sendSeivice.MessageCode
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    
    }
}
