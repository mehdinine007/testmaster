using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.UserManagement.UserDto;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/RolePermissionController/[action]")]
    //[UserAuthorization]
    public class RolePermissionController : Controller, IRolePermissionService
    {
        private readonly IRolePermissionService _rolePermission;

        public RolePermissionController(IRolePermissionService rolePermission)
            => _rolePermission = rolePermission;
        
        [HttpGet]
        public async Task<List<RolePermissionDto>> GetList()
        => await _rolePermission.GetList();

        [HttpGet]
        public async Task InsertList()
            => await _rolePermission.InsertList();
    }
}
