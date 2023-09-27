using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.Grpc.UserGrpcClient;

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
            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("Grpc:UserUrl"), new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new UserServiceGrpc.UserServiceGrpcClient(channel);
            var user = client.GetUserById(new GetUserModel() { UserId = userId.ToString() });
            if (user.BankId == 0)
                return null;
            return new UserDto

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

                SurName = user.SurName

            };

        }


    }
}
