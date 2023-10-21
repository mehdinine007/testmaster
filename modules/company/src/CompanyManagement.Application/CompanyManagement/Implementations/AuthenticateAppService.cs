using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement.Dtos;
using CompanyManagement.Application.Contracts.CompanyManagement.IServices;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
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

    public async Task<AuthenticateResponseDto> Authenticate(AuthenticateReqDto input)
    {
        return await _userGrpcClientService.Athenticate(input);
    }

}
