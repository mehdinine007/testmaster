using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/RoleService/[action]")]
    //[UserAuthorization]
    public class RoleController: Controller
    {

        private readonly IRoleService _roleService;
        public RoleController(IRoleService workFlowRoleService)
        => _roleService = workFlowRoleService;


        [HttpGet]
        public Task<RoleDto> GetById(int id)
    => _roleService.GetById(id);


        [HttpGet]
        public Task<List<RoleDto>> GetList()
        => _roleService.GetList();


        [HttpPost]
        public Task<RoleDto> Add(RoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _roleService.Add(workFlowRoleCreateOrUpdateDto);

        [HttpPut]
        public Task<RoleDto> Update(RoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _roleService.Update(workFlowRoleCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _roleService.Delete(id);
    

}
}
