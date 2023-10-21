using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Bson;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using System.Threading.Tasks;
using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement.Dtos;
using Esale.Share.Authorize;

namespace CompanyManagement.HttpApi.CompanyManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/TokenAuthService/[action]")]
//[UserAuthorization]
public class TokenAuthController : Controller
{
    private readonly IAuthenticateAppService _authenticateAppService;


    public TokenAuthController(IAuthenticateAppService authenticateAppService)
    {
        _authenticateAppService = authenticateAppService;
    }


    [HttpPost]
    public async Task<AuthenticateResult> Authenticate(AuthenticateReqDto model)
    {
        var auth = await _authenticateAppService.Authenticate(model);
        if (auth.Success)
        {
            return auth.Data;
        }
        throw new UserFriendlyException(auth.Message, auth.ErrorCode.ToString());
    }


}
