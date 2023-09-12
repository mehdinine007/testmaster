

using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Auditing;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Implementations
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/RoleOrganizationChartService/[action]")]
    //[UserAuthorization]
    public class RoleOrganizationChartController:Controller
    {
        private readonly IRoleOrganizationChartService _roleOrganizationChartService;
        public RoleOrganizationChartController(IRoleOrganizationChartService roleOrganizationChartService)
        => _roleOrganizationChartService = roleOrganizationChartService;
        [HttpPost]
        public Task<RoleOrganizationChartDto> Add(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto)
       => _roleOrganizationChartService.Add(roleOrganizationChartCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        => _roleOrganizationChartService.Delete(id);
        [HttpGet]
        public Task<RoleOrganizationChartDto> GetById(int id)
        => _roleOrganizationChartService.GetById(id);
        [HttpGet]
        public Task<List<RoleOrganizationChartDto>> GetList()
       => _roleOrganizationChartService.GetList();
        [HttpPut]
        public Task<RoleOrganizationChartDto> Update(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto)
       => _roleOrganizationChartService.Update(roleOrganizationChartCreateOrUpdateDto);  
    }
}
