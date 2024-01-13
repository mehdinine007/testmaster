using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.Models;
using MongoDB.Bson;
using UserManagement.Application.UserManagement.Implementations;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/PermissionDefinitionController/[action]")]
    //[UserAuthorization]
    public class PermissionDefinitionController : Controller
    {
        private readonly IPermissionDefinitionService _permission;

        public PermissionDefinitionController(IPermissionDefinitionService rolePermission)
            => _permission = rolePermission;
        [HttpDelete]
        public async Task<bool> Delete(ObjectId Id)
                => await _permission.Delete(Id);
        
        [HttpGet]
        public async Task<PermissionDefinitionDto> GetById(ObjectId Id)
         => await _permission.GetById(Id);

        [HttpGet]
        public async Task<List<PermissionDefinitionDto>> GetList()
            => await _permission.GetList();

        [HttpPost]
        public async Task<PermissionDefinitionDto> Add(PermissionDefinitionDto permission)
            => await _permission.Add(permission);
        [HttpGet]
        public async Task InsertList()
            => await _permission.InsertList();
        [HttpPut]
        public async Task<PermissionDefinitionDto> Update(PermissionDefinitionDto input)
            => await _permission.Update(input);

        [HttpPost]
        public async Task SeedPermissions()
          => await _permission.SeedPermissions();
    }
}
