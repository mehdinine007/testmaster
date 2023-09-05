using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
using WorkFlowManagement.Domain.WorkFlowManagement;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class ActivityService : ApplicationService, IActivityService
    {

        private readonly IRepository<Activity, int> _activityRepository;
        private readonly IRepository<Scheme, int> _schemeRepository;


        public ActivityService(IRepository<Activity, int> activityRepository, IRepository<Scheme, int> schemeRepository)
        {
            _activityRepository = activityRepository;
            _schemeRepository = schemeRepository;
        }

        public async Task<ActivityDto> Add(ActivityCreateOrUpdateDto activityCreateOrUpdateDto)
        {

            await Validation(null, activityCreateOrUpdateDto, null);
            var activity = ObjectMapper.Map<ActivityCreateOrUpdateDto, Activity>(activityCreateOrUpdateDto);
            var entity = await _activityRepository.InsertAsync(activity, autoSave: true);
            return ObjectMapper.Map<Activity, ActivityDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var activity = await Validation(id, null, null);
            await _activityRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ActivityDto> GetById(int? id)
        {
            var activity = await Validation(id, null, null);
            var activityDto = ObjectMapper.Map<Activity, ActivityDto>(activity);
            return activityDto;
        }

        public async Task<ActivityDto> GetBySchemeId(int schemeId)
        {
            var activity = await Validation(null, null, schemeId);
            var activityDto = ObjectMapper.Map<Activity, ActivityDto>(activity);
            return activityDto;
        }



        public async Task<List<ActivityDto>> GetList()
        {
            var activity = (await _activityRepository.GetQueryableAsync()).Include(x => x.Scheme).ToList();
            var activityDto = ObjectMapper.Map<List<Activity>, List<ActivityDto>>(activity);
            return activityDto;
        }

        public async Task<ActivityDto> Update(ActivityCreateOrUpdateDto activityCreateOrUpdateDto)
        {
            var activity = await Validation(activityCreateOrUpdateDto.Id, activityCreateOrUpdateDto, null);
            activity.Title = activityCreateOrUpdateDto.Title;
            activity.Type = activityCreateOrUpdateDto.Type;
            activity.FlowType = activityCreateOrUpdateDto.FlowType;
            activity.Entity = activityCreateOrUpdateDto.Entity;
            activity.SchemeId = activityCreateOrUpdateDto.SchemeId;
            var entity = await _activityRepository.UpdateAsync(activity);
            return ObjectMapper.Map<Activity, ActivityDto>(entity);
        }

        private async Task<Activity> Validation(int? id, ActivityCreateOrUpdateDto activityCreateOrUpdateDto, int? schemeId)
        {
            var activity = new Activity();
            var activityQuery = (await _activityRepository.GetQueryableAsync()).Include(x => x.Scheme);
            if (schemeId is not null)
            {
                activity= activityQuery.FirstOrDefault(x => x.FlowType == FlowTypeEnum.Start && x.SchemeId == schemeId);
                if (activity is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivityNotFound, WorkFlowConstant.ActivityNotFoundId);
                }
            }
            if (id != null)
            {
                activity = activityQuery.FirstOrDefault(x => x.Id == id);
                if (activity is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivityNotFound, WorkFlowConstant.ActivityNotFoundId);
                }
            }
            if (activityCreateOrUpdateDto != null)
            {
                var schemeQuery = await _schemeRepository.GetQueryableAsync();
                var scheme = schemeQuery.FirstOrDefault(x => x.Id == activityCreateOrUpdateDto.SchemeId);
                if (scheme is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.SchemeNotFound, WorkFlowConstant.SchemeNotFoundId);
                }
            }
            return activity;
        }


    }
}
