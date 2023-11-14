using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Grpc.UserGrpcClient;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

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
            var auth = client.Authenticate(new Application.Grpc.UserGrpcClient.AuthenticateRequest() { UserNameOrEmailAddress = input.UserNameOrEmailAddress, Password = input.Password });
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
