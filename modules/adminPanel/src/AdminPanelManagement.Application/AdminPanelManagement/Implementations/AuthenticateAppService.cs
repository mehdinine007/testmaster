using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using IFG.Core.Infrastructures.TokenAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace AdminPanelManagement.Application.AdminPanelManagement.Implementations
{
    public class AuthenticateAppService: ApplicationService, IAuthenticateAppService
    {
        private readonly IUserGrpcClientService _userGrpcClientService;


        public AuthenticateAppService(IUserGrpcClientService userGrpcClientService)
        {
            _userGrpcClientService = userGrpcClientService;
        }

        public async Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto input)
        {
            var auth = await _userGrpcClientService.Athenticate(input);
            if (auth.Success)
            {
                return auth.Data;
            }
            throw new UserFriendlyException(auth.Message, auth.ErrorCode.ToString());

        }
    }
}
