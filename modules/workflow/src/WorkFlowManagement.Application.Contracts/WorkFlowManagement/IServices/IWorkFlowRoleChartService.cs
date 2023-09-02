using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IWorkFlowRoleChartService:IApplicationService
    {
        Task<WorkFlowRoleChartDto> GetById(int id);
        Task<WorkFlowRoleChartDto> Add(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto);
        Task<WorkFlowRoleChartDto> Update(WorkFlowRoleChartCreateOrUpdateDto workFlowRoleChartCreateOrUpdateDto);
        Task<List<WorkFlowRoleChartDto>> GetList();
        Task<bool> Delete(int id);

    }
}
