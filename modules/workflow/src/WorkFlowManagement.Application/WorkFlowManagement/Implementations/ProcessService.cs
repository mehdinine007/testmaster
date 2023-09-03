using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Activity = WorkFlowManagement.Domain.WorkFlowManagement.Activity;
using Process = WorkFlowManagement.Domain.WorkFlowManagement.Process;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class ProcessService : ApplicationService, IProcessService
    {
        private readonly IRepository<OrganizationChart, int> _organizationChartRepository;
        private readonly IRepository<Scheme, int> _schemeRepository;
        private readonly IRepository<Process, Guid> _processRepository;
        private readonly IRepository<Activity, int> _activityRepository;
        public ProcessService(IRepository<OrganizationChart, int> organizationChartRepository, IRepository<Scheme, int> schemeRepository, IRepository<Process, Guid> processRepository, IRepository<Activity, int> activityRepository)
        {
            _organizationChartRepository = organizationChartRepository;
            _schemeRepository = schemeRepository;
            _processRepository = processRepository;
            _activityRepository = activityRepository;
        }


        public async Task<ProcessDto> Add(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        {
            await Validation(null, processCreateOrUpdateDto);
            var uid = Guid.NewGuid();
            processCreateOrUpdateDto.Id = uid;
            var duplicateUid = (await _processRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == uid);
            while (duplicateUid != null)
            {
                uid = Guid.NewGuid();
                processCreateOrUpdateDto.Id = uid;
                duplicateUid = (await _processRepository.GetQueryableAsync()).FirstOrDefault(x => x.Id == uid);

            }
            var process = ObjectMapper.Map<ProcessCreateOrUpdateDto, Process>(processCreateOrUpdateDto);
            var entity = await _processRepository.InsertAsync(process, autoSave: true);
            return ObjectMapper.Map<Process, ProcessDto>(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            var process = await Validation(id, null);
            await _processRepository.DeleteAsync(id);
            return true;
        }

        public async Task<ProcessDto> GetById(Guid id)
        {
            var process = await Validation(id, null);
            var processDto = ObjectMapper.Map<Process, ProcessDto>(process);
            return processDto;
        }

        public async Task<List<ProcessDto>> GetList()
        {
            var process = (await _processRepository.GetQueryableAsync()).Include(x => x.Scheme).ToList();
            var processDto = ObjectMapper.Map<List<Process>, List<ProcessDto>>(process);
            return processDto;
        }

        public async Task<ProcessDto> Update(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        {
            var process = await Validation(processCreateOrUpdateDto.Id, processCreateOrUpdateDto);
            process.Title = processCreateOrUpdateDto.Title;
            process.Subject = processCreateOrUpdateDto.Subject;
            process.Description = processCreateOrUpdateDto.Description;
            process.State = processCreateOrUpdateDto.State;
            process.SchemeId = processCreateOrUpdateDto.SchemeId;
            process.Status = processCreateOrUpdateDto.Status;
            var entity = await _processRepository.UpdateAsync(process);
            return ObjectMapper.Map<Process, ProcessDto>(entity);
        }

        private async Task<Process> Validation(Guid? id, ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        {
            var process = new Process();
            var processQuery = (await _processRepository.GetQueryableAsync());
            if (id != null)
            {
                process = processQuery.FirstOrDefault(x => x.Id == id);
                if (process is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.ProcessNotFound, WorkFlowConstant.ProcessNotFoundId);
                }
            }

            if (processCreateOrUpdateDto != null)
            {
                var activityQuery = (await _activityRepository.GetQueryableAsync()).AsNoTracking();
                if (processCreateOrUpdateDto.PreviousActivityId != null)
                {
                    var previousActivity = activityQuery.FirstOrDefault(x => x.Id == processCreateOrUpdateDto.PreviousActivityId);
                    if (previousActivity is null)
                        throw new UserFriendlyException(WorkFlowConstant.PreviousActivityNotFound, WorkFlowConstant.PreviousActivityNotFoundId);
                }
                var activity = activityQuery.FirstOrDefault(x => x.Id == processCreateOrUpdateDto.ActivityId);
                if (activity is null)
                    throw new UserFriendlyException(WorkFlowConstant.ActivityNotFound, WorkFlowConstant.ActivityNotFoundId);

                var schemeQuery = await _schemeRepository.GetQueryableAsync();
                var scheme= schemeQuery.FirstOrDefault(x=>x.Id== processCreateOrUpdateDto.SchemeId);
                if (scheme is null)
                    throw new UserFriendlyException(WorkFlowConstant.SchemeNotFound, WorkFlowConstant.SchemeNotFoundId);


                var organizationChartQuery = (await _organizationChartRepository.GetQueryableAsync()).AsNoTracking();
                var createdOrganizationChart= organizationChartQuery.FirstOrDefault(x=>x.Id== processCreateOrUpdateDto.CreatedOrganizationChartId);
                if (createdOrganizationChart is null)
                    throw new UserFriendlyException(WorkFlowConstant.CreatedOrganizationChartNotFound, WorkFlowConstant.CreatedOrganizationChartNotFoundId);
                var OrganizationChart= organizationChartQuery.FirstOrDefault(x=>x.Id== processCreateOrUpdateDto.OrganizationChartId);
                if (OrganizationChart is null)
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                if (processCreateOrUpdateDto.PreviousOrganizationChartId != null)
                {
                    var PreviousOrganizationChart = organizationChartQuery.FirstOrDefault(x => x.Id == processCreateOrUpdateDto.PreviousOrganizationChartId);
                    if (PreviousOrganizationChart is null)
                    {
                        throw new UserFriendlyException(WorkFlowConstant.PreviousOrganizationChartNotFound, WorkFlowConstant.PreviousOrganizationChartNotFoundId);
                    }
                }
            }

            return process;
        }



    }
}
