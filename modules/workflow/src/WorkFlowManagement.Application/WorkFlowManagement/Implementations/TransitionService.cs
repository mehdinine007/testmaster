using AutoMapper.Internal.Mappers;
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
    public class TransitionService : ApplicationService, ITransitionService
    {
        private readonly IRepository<Transition, int> _transitionRepository;
        private readonly IRepository<Activity, int> _activityRepository;
        private readonly IRepository<Scheme, int> _schemeRepository;
        public TransitionService(IRepository<Transition, int> transitionRepository, IRepository<Activity, int> activityRepository, IRepository<Scheme, int> schemeRepository)
        {
            _transitionRepository = transitionRepository;
            _activityRepository = activityRepository;
            _schemeRepository = schemeRepository;
        }


        public async Task<TransitionDto> Add(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto)
        {
            await Validation(null, transitionCreateOrUpdateDto);
            var transition = ObjectMapper.Map<TransitionCreateOrUpdateDto, Transition>(transitionCreateOrUpdateDto);
            var entity = await _transitionRepository.InsertAsync(transition, autoSave: true);
            return ObjectMapper.Map<Transition, TransitionDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var transition = await Validation(id, null);
            await _transitionRepository.DeleteAsync(id);
            return true;
        }

        public async Task<TransitionDto> GetById(int id)
        {
            var transition = await Validation(id, null);
            var transitionDto = ObjectMapper.Map<Transition, TransitionDto>(transition);
            return transitionDto;
        }

        public async Task<List<TransitionDto>> GetList()
        {
            var transition = (await _transitionRepository.GetQueryableAsync()).Include(x => x.ActivitySource).Include(x => x.ActivityTarget).ToList();
            var transitionDto = ObjectMapper.Map<List<Transition>, List<TransitionDto>>(transition);
            return transitionDto;
        }

        public async Task<TransitionDto> Update(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto)
        {
            var transition = await Validation(transitionCreateOrUpdateDto.Id, transitionCreateOrUpdateDto);
            transition.ActivitySourceId = transitionCreateOrUpdateDto.ActivitySourceId;
            transition.ActivityTargetId = transitionCreateOrUpdateDto.ActivityTargetId;
   
            var entity = await _transitionRepository.UpdateAsync(transition);
            return ObjectMapper.Map<Transition, TransitionDto>(entity);
        }


        private async Task<Transition> Validation(int? id, TransitionCreateOrUpdateDto transitionCreateOrUpdateDto)
        {
            var transition = new Transition();
            var transitionQuery = (await _transitionRepository.GetQueryableAsync()).Include(x => x.ActivitySource).Include(x => x.ActivityTarget);
            if (id != null)
            {
                transition = transitionQuery.FirstOrDefault(x => x.Id == id);
                if (transition is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.TransitionNotFound, WorkFlowConstant.TransitionNotFoundId);
                }
            }
            if (transitionCreateOrUpdateDto != null)
            {
                var activity = (await _activityRepository.GetQueryableAsync()).AsNoTracking();
                var activtySorce = activity.FirstOrDefault(x => x.Id == transitionCreateOrUpdateDto.ActivitySourceId);
                    if (activtySorce is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivitySourceNotFound, WorkFlowConstant.ActivitySourceNotFoundId);
                }

                var activtyTarget = activity.FirstOrDefault(x => x.Id == transitionCreateOrUpdateDto.ActivityTargetId);
                    if (activtyTarget is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ActivityTargetNotFound, WorkFlowConstant.ActivityTargetNotFoundId);
                }

            }
            return transition;
        }

    }
}
