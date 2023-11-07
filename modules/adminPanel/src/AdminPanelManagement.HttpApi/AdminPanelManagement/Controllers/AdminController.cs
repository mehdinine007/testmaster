using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using AdminPanelManagement.EntityFrameworkCore;
using AdminPanelManagement.Application.AdminPanelManagement.Implementations;
using Microsoft.AspNetCore.Mvc;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.IServices;
using AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/AdminService/[action]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        public Task<UserInfo_CustomerOrderDto> GetCustomerOrderList(string nationalCode)
        =>_adminService.GetCustomerOrderList(nationalCode);
    }
}
