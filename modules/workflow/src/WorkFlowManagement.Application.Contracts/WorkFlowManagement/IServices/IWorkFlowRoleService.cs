using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IWorkFlowRoleService: IApplicationService
    {

        Task<WorkFlowRoleDto> GetById(int id);
        Task<WorkFlowRoleDto> Add(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto);
        Task<WorkFlowRoleDto> Update(WorkFlowRoleCreateOrUpdateDto workFlowRoleCreateOrUpdateDto);
        Task<List<WorkFlowRoleDto>> GetList();
        Task<bool> Delete(int id);
    }
}
