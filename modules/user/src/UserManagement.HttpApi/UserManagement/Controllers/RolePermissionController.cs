using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/RolePermissionController/[action]")]
    //[UserAuthorization]
    public class RolePermissionController : Controller//, IRolePermissionService
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

        [HttpPost]
        public async Task<RolePermissionDto> Add(RolePermissionDto dto)
            => await _rolePermission.Add(dto);

        [HttpPut]
        public async Task<RolePermissionDto> Update(RolePermissionDto dto)
              => await _rolePermission.Update(dto);

        [HttpGet]
        public async Task<RolePermissionDto> GetById(ObjectId id)
              => await _rolePermission.GetById(id);
        [HttpDelete]
        public async Task<bool> Delete(ObjectId id)
              => await _rolePermission.Delete(id);


    }
}
