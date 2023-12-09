using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using System.Threading.Tasks;
using IFG.Core.Infrastructures.TokenAuth;
using CompanyManagement.Application.Contracts;

namespace CompanyManagement.HttpApi.CompanyManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/TokenAuth/Auth/[action]")]
//[UserAuthorization]
public class TokenAuthController : Controller
{
    private readonly IAuthenticateAppService _authenticateAppService;


    public TokenAuthController(IAuthenticateAppService authenticateAppService)
    {
        _authenticateAppService = authenticateAppService;
    }


    [HttpPost]
    public async Task<AuthenticateResultModel> Auth(AuthenticateReqDto model)
    {
        return await _authenticateAppService.Authenticate(model);
    }
}
