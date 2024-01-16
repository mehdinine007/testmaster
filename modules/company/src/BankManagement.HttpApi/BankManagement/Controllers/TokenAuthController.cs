using CompanyManagement.Application.Contracts;
using CompanyManagement.Application.Contracts.CompanyManagement.Services;
using IFG.Core.Infrastructures.TokenAuth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace CompanyManagement.HttpApi.BankManagement.Controllers
{
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
}
