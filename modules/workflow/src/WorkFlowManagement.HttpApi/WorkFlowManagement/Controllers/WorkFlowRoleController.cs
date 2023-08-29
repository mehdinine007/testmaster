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
    public class WorkFlowRoleController: Controller
    {

        private readonly IWorkFlowRoleService _workFlowRoleService;
        public WorkFlowRoleController(IWorkFlowRoleService workFlowRoleService)
        => _workFlowRoleService = workFlowRoleService;


        [HttpGet]
        public Task<WorkFlowRoleDto> GetById(int id)
    => _workFlowRoleService.GetById(id);


        [HttpGet]
        public Task<List<WorkFlowRoleDto>> GetList()
        => _workFlowRoleService.GetList();


        [HttpPost]
        public Task<WorkFlowRoleDto> Add(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _workFlowRoleService.Add(workFlowRoleCreateOrUpdateDto);

        [HttpPut]
        public Task<WorkFlowRoleDto> Update(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto)
        => _workFlowRoleService.Update(workFlowRoleCreateOrUpdateDto);

        [HttpDelete]
        public Task<bool> Delete(int id)
        => _workFlowRoleService.Delete(id);
    

}
}
