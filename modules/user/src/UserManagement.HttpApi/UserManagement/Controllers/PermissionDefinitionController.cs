using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.Models;
using MongoDB.Bson;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/PermissionDefinitionController/[action]")]
    //[UserAuthorization]
    public class PermissionDefinitionController : Controller, IPermissionDefinitionService
    {
        private readonly IPermissionDefinitionService _permission;

        public PermissionDefinitionController(IPermissionDefinitionService rolePermission)
            => _permission = rolePermission;
        [HttpPost]
        public async Task<PermissionDefinitionDto> Add(PermissionDefinitionDto permission)
                => await _permission.Add(permission);
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
        public async Task<PermissionDefinitionDto> Insert(PermissionDefinitionDto permission)
            => await _permission.Insert(permission);
        [HttpGet]
        public async Task InsertList()
            => await _permission.InsertList();
        [HttpPut]
        public async Task<PermissionDefinitionDto> Update(PermissionDefinitionDto input)
            => await _permission.Update(input);
    }
}
