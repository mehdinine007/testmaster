using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;
using Microsoft.AspNetCore.Mvc;

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
