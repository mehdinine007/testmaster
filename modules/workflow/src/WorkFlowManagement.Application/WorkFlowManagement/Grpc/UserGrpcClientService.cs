#region NS
using Grpc.Net.Client;
using IFG.Core.Caching;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.Grpc.UserGrpcClient;
using WorkFlowManagement.Application.WorkFlowManagement.Constants;
#endregion

namespace WorkFlowManagement.Application.WorkFlowManagement.Grpc
{
    public class UserGrpcClientService : ApplicationService, IUserGrpcClientService
    {
        private IConfiguration _configuration { get; set; }
        public UserGrpcClientService(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }


        public async Task<UserDto> GetUserById(Guid userId)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:UserUrl"), new GrpcChannelOptions 
            { HttpHandler = httpHandler });
            
     
            var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
            var user = client.GetUserById(new GetUserModel() { UserId = userId.ToString() });
            if (user.Uid is null || user.Uid == "")
                return null;

            var userDto = new UserDto
            {
                AccountNumber = user.AccountNumber,
                BankId = user.BankId,
                BirthCityId = user.BirthCityId,
                BirthProvinceId = user.BirthProvinceId,
                HabitationCityId = user.HabitationCityId,
                HabitationProvinceId = user.HabitationProvinceId,
                IssuingCityId = user.IssuingCityId,
                IssuingProvinceId = user.IssuingProvinceId,
                NationalCode = user.NationalCode,
                Shaba = user.Shaba,
                MobileNumber = user.MobileNumber,
                CompanyId = user.CompanyId,
                Name = user.Name,
                SurName = user.SurName,
                Uid = user.Uid,
            };
      
            return userDto;
        }

        public async Task<AuthenticateResponseDto> Athenticate(AuthenticateReqDto input)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:UserUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
            var auth = client.Authenticate(new AuthenticateRequest() { UserNameOrEmailAddress = input.UserNameOrEmailAddress, Password = input.Password });
            var res = new AuthenticateResponseDto();

            if (!auth.Success)
            {
                res.Success = auth.Success;
                res.Message = auth.Message;
                res.ErrorCode = auth.ErrorCode.Value;
                return res;
            }

            res.Success = true;
            res.Data.AccessToken = auth.Data.AccessToken;
            res.Data.EncryptedAccessToken = auth.Data.EncryptedAccessToken;
            res.Data.ExpireInSeconds = auth.Data.ExpireInSeconds.Value;
            return res;
        }
    }
}
