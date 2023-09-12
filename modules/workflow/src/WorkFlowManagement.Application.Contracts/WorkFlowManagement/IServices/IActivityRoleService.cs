using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IActivityRoleService: IApplicationService
    {

        Task<ActivityRoleDto> GetById(int id);
        Task<ActivityRoleDto> Add(ActivityRoleCreateOrUpdateDto activityRoleCreateOrUpdate);
        Task<ActivityRoleDto> Update(ActivityRoleCreateOrUpdateDto activityRoleCreateOrUpdate);
        Task<List<ActivityRoleDto>> GetList();
        Task<bool> Delete(int id);
        Task<ActivityRoleDto> GetByActivityId(int activityId);
       

    }
}
