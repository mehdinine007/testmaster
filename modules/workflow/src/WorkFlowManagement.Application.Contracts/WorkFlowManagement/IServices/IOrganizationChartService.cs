using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IOrganizationChartService: IApplicationService
    {
        Task<OrganizationChartDto> GetById(int id);
        Task<OrganizationChartDto> Add(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto);
        Task<OrganizationChartDto> Update(OrganizationChartCreateOrUpdateDto organizationChartCreateOrUpdateDto);
        Task<List<OrganizationChartDto>> GetList();
        Task<bool> Delete(int id);

    }
}
