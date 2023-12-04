using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement.IServices;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using IFG.Core.Infrastructures.TokenAuth;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.UserManagement.Implementations;

public class AuthenticateAppService : ApplicationService, IAuthenticateAppService
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
