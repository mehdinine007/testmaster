﻿using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;

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

        [HttpGet]
        public async Task<List<PermissionDefinitionDto>> GetList()
            => await _permission.GetList();

        [HttpPost]
        public async Task<List<PermissionDefinitionDto>> Insert(PermissionDefinitionDto permission)
            => await _permission.Insert(permission);
    }
}
