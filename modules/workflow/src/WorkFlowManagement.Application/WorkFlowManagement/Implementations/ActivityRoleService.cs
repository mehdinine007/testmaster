using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class ActivityRoleService : ApplicationService, IActivityRoleService
    {
        private readonly IRepository<ActivityRole, int> _activityRoleRepository;
        private readonly IRepository<Activity, int> _activityRepository;
        private readonly IRepository<Role, int> _roleRepository;
        public ActivityRoleService(IRepository<ActivityRole, int> activityRoleRepository, IRepository<Activity, int> activityRepository, IRepository<Role, int> roleRepository)
        {
            _activityRepository = activityRepository;
            _roleRepository = roleRepository;
            _activityRoleRepository = activityRoleRepository;

        }


        public async Task<ActivityRoleDto> Add(ActivityRoleCreateOrUpdateDto activityRoleCreateOrUpdate)
        {
            await Validation(null, activityRoleCreateOrUpdate);
            var activityRole = ObjectMapper.Map<ActivityRoleCreateOrUpdateDto, ActivityRole>(activityRoleCreateOrUpdate);
            var entity = await _activityRoleRepository.InsertAsync(activityRole, autoSave: true);
            return ObjectMapper.Map<ActivityRole, ActivityRoleDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var activityRole = await Validation(id, null);
            await _activityRoleRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ActivityRoleDto> GetById(int id)
        {
            var activityRole = await Validation(id, null);
            var activityRoleDto = ObjectMapper.Map<ActivityRole, ActivityRoleDto>(activityRole);
            return activityRoleDto;
        }

        public async Task<ActivityRoleDto> GetByActivityId(int activityId)
        {
            var activityRole = await _activityRoleRepository.FirstOrDefaultAsync(x => x.ActivityId == activityId);
            if (activityRole == null)
                throw new UserFriendlyException(WorkFlowConstant.ActivityRoleNotFound, WorkFlowConstant.ActivityRoleNotFoundId);
            var activityRoleDto = ObjectMapper.Map<ActivityRole, ActivityRoleDto>(activityRole);
            return activityRoleDto;
        }


        public async Task<List<ActivityRoleDto>> GetList()
        {
            var activityRole = (await _activityRoleRepository.GetQueryableAsync()).Include(x => x.Activity).Include(x => x.Role).ToList();
            var activityRoleDto = ObjectMapper.Map<List<ActivityRole>, List<ActivityRoleDto>>(activityRole);
            return activityRoleDto;
        }

        public async Task<ActivityRoleDto> Update(ActivityRoleCreateOrUpdateDto activityRoleCreateOrUpdate)
        {
            var activityRole = await Validation(activityRoleCreateOrUpdate.Id, activityRoleCreateOrUpdate);
            activityRole.ActivityId = activityRoleCreateOrUpdate.ActivityId;
            activityRole.RoleId = activityRoleCreateOrUpdate.RoleId;
            var entity = await _activityRoleRepository.UpdateAsync(activityRole);
            return ObjectMapper.Map<ActivityRole, ActivityRoleDto>(entity);
        }


        private async Task<ActivityRole> Validation(int? id, ActivityRoleCreateOrUpdateDto activityRoleCreateOrUpdate)
        {
            var activityRole = new ActivityRole();
            var activityRoleQuery = (await _activityRoleRepository.GetQueryableAsync()).Include(x => x.Activity).Include(x => x.Role);
            if (id != null)
            {
                activityRole = activityRoleQuery.FirstOrDefault(x => x.Id == id);
                if (activityRole is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivityRoleNotFound, WorkFlowConstant.ActivityRoleNotFoundId);
                }
            }
            if (activityRoleCreateOrUpdate != null)
            {
                var activityQuery = await _activityRepository.GetQueryableAsync();
                var activity = activityQuery.FirstOrDefault(x => x.Id == activityRoleCreateOrUpdate.ActivityId);
                if (activity is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivityNotFound, WorkFlowConstant.ActivityNotFoundId);
                }


                var roleQuery = await _roleRepository.GetQueryableAsync();
                var role = roleQuery.FirstOrDefault(x => x.Id == activityRoleCreateOrUpdate.RoleId);
                if (role is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.RoleNotFound, WorkFlowConstant.RoleNotFoundId);
                }
            }
            return activityRole;
        }


    }
}
