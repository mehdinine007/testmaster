using Volo.Abp.Auditing;
using Volo.Abp;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using Microsoft.AspNetCore.Mvc;
using IFG.Core.Infrastructures.TokenAuth;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/TokenAuth/[action]")]

    public class TokenAuthController : Controller
    {
        private readonly IAuthenticateAppService _authenticateAppService;


        public TokenAuthController(IAuthenticateAppService authenticateAppService)
        {
            _authenticateAppService = authenticateAppService;
        }


        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate(AuthenticateReqDto model)
        {
            return await _authenticateAppService.Authenticate(model);
        }


    }
}
