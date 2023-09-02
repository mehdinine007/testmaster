using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IRoleOrganizationChartService:IApplicationService
    {
        Task<RoleOrganizationChartDto> GetById(int id);
        Task<RoleOrganizationChartDto> Add(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto);
        Task<RoleOrganizationChartDto> Update(RoleOrganizationChartCreateOrUpdateDto roleOrganizationChartCreateOrUpdateDto);
        Task<List<RoleOrganizationChartDto>> GetList();
        Task<bool> Delete(int id);

    }
}
