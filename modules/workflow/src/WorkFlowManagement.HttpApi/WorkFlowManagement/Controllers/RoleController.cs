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
    [Route("api/services/app/WorkFlowRoleService/[action]")]
    //[UserAuthorization]
    public class RoleController: Controller
    {

        private readonly IRoleService _workFlowRoleService;
        public RoleController(IRoleService workFlowRoleService)
        => _workFlowRoleService = workFlowRoleService;


        [HttpGet]
        public Task<RoleDto> GetById(int id)
    => _workFlowRoleService.GetById(id);


        [HttpGet]
        public Task<List<RoleDto>> GetList()
        => _workFlowRoleService.GetList();


        [HttpPost]
        public Task<RoleDto> Add(RoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _workFlowRoleService.Add(workFlowRoleCreateOrUpdateDto);

        [HttpPut]
        public Task<RoleDto> Update(RoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _workFlowRoleService.Update(workFlowRoleCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _workFlowRoleService.Delete(id);
    

}
}
