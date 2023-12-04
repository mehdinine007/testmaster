using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using System.Threading.Tasks;
using IFG.Core.Infrastructures.TokenAuth;
using CompanyManagement.Application.Contracts;

namespace OrderManagement.HttpApi.OrderManagement.Controllers;

[Route("api/TokenAuth/[action]")]
[RemoteService]
[DisableAuditing]
public class TokenAuthController : AbpController
{
    private readonly IAuthenticateAppService _authenticateAppService;

    public TokenAuthController(IAuthenticateAppService authenticateAppService)
        => _authenticateAppService = authenticateAppService;

    [HttpPost]
    public async Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto input)
        => await _authenticateAppService.Authenticate(input);
}
