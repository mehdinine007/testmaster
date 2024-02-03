using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement;

namespace OrderManagement.HttpApi.OrderManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/UserDataAccessService/[action]")]
    public class OldCarController : Controller
    {
        private readonly IUserDataAccessService _userDataAccessService;
        public OldCarController(IUserDataAccessService userDataAccessService)
        {
            _userDataAccessService = userDataAccessService;
        }

        [HttpGet]
        public async Task<List<OldCarDto>> OldCarGetList(string nationalcode)
        {
            return await _userDataAccessService.OldCarGetList(nationalcode);
        }
    }
}
