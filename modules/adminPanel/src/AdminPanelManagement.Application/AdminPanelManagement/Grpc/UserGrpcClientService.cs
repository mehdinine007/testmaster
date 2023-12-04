using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using AdminPanelManagement.Application.Grpc.UserGrpcClient;
using Volo.Abp.Application.Services;
using IFG.Core.Infrastructures.TokenAuth;
using CompanyManagement.Application.Contracts;

namespace AdminPanelManagement.Application.AdminPanelManagement.Grpc
{
    public class UserGrpcClientService : ApplicationService, IUserGrpcClientService
    {
        private IConfiguration _configuration { get; set; }
        public UserGrpcClientService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:UserUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
            var auth = client.Authenticate(new Application.Grpc.UserGrpcClient.AuthenticateRequest() { UserNameOrEmailAddress = input.userID, Password = input.userPWD });
            var res = new AuthenticateResponseDto();
            if (!auth.Success)
            {
                res.Success = auth.Success;
                res.Message = auth.Message;
                res.ErrorCode = auth.ErrorCode.Value;
                return res;
            }

            res.Success = auth.Success;
            res.Data = new AuthenticateResultModel();
            res.Data.AccessToken = auth.Data.AccessToken;
            res.Data.EncryptedAccessToken = auth.Data.EncryptedAccessToken;
            res.Data.ExpireInSeconds = auth.Data.ExpireInSeconds.Value;

            return res;
        }
    }
}
