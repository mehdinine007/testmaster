using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AdminPanelService/[action]")]
    //[UserAuthorization]
    public class AdminPanelController: Controller
    {

       private readonly IAdminPanelService _adminPanelService;
        public AdminPanelController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }
        [HttpGet]
        public Task<UserInfo_CustomerOrderDto> GetCustomerOrderList(string nationalCode)
        => _adminPanelService.GetCustomerOrderList(nationalCode);

    }
}
