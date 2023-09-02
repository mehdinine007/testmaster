

using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Auditing;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Implementations
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/WorkFlowRoleChartService/[action]")]
    //[UserAuthorization]
    public class WorkFlowRoleChartController:Controller
    {
        private readonly IWorkFlowRoleChartService _workFlowRoleChartService;
        public WorkFlowRoleChartController(IWorkFlowRoleChartService workFlowRoleChartService)
        => _workFlowRoleChartService = workFlowRoleChartService;
        [HttpPost]
        public Task<WorkFlowRoleChartDto> Add(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto)
       =>_workFlowRoleChartService.Add(workFlowRoleChartCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_workFlowRoleChartService.Delete(id);
        [HttpGet]
        public Task<WorkFlowRoleChartDto> GetById(int id)
        =>_workFlowRoleChartService.GetById(id);
        [HttpGet]
        public Task<List<WorkFlowRoleChartDto>> GetList()
       =>_workFlowRoleChartService.GetList();
        [HttpPut]
        public Task<WorkFlowRoleChartDto> Update(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto)
       =>_workFlowRoleChartService.Update(workFlowRoleChartCreateOrUpdateDto);  
    }
}
