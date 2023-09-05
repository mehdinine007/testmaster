using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using Org.BouncyCastle.Asn1.TeleTrust;
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
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
using WorkFlowManagement.Domain.WorkFlowManagement;
using Activity = WorkFlowManagement.Domain.WorkFlowManagement.Activity;
using Process = WorkFlowManagement.Domain.WorkFlowManagement.Process;

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class ProcessService : ApplicationService, IProcessService
    {
        private readonly IRepository<Process, Guid> _processRepository;
        private readonly ISchemeService _schemeService;
        private readonly IActivityService _activityService;
        private readonly ICommonAppService _commonAppService;
        private readonly IOrganizationPositionService _organizationPositionService;
        private readonly IOrganizationChartService _organizationChartService;
        public ProcessService(IRepository<Process
            , Guid> processRepository,ISchemeService schemeService 
            , IActivityService activityService
            , ICommonAppService commonAppService
            , IOrganizationPositionService organizationPositionService
            , IOrganizationChartService organizationChartService

            )
        {
            _processRepository = processRepository;
            _schemeService = schemeService;
            _activityService = activityService;
            _commonAppService = commonAppService;
            _organizationPositionService = organizationPositionService;
            _organizationChartService= organizationChartService;
        
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

        public async Task<bool> StartProcess(int schemeId)
        {
            Guid userId = new Guid("9e53dd17-1d3d-4a29-9ca7-ca980f240a73");
            //var currentUserId = _commonAppService.GetUserId();
            var scheme = await _schemeService.GetById(schemeId);
            var activity =await  _activityService.GetBySchemeId(schemeId);
            var organizationPosition =await _organizationPositionService.GetByPersonId(userId);
           
            var process = new Process()
            {
                SchemeId = schemeId,
                Status = StatusEnum.Initialize,
                State = StateEnum.Start,
                Title = scheme.Title,
                Subject="",
                Description="",
                ActivityId = activity.Id,
                PersonId= userId,
                CreatedPersonId= userId,
                OrganizationChartId = organizationPosition.OrganizationChartId,
                CreatedOrganizationChartId= organizationPosition.OrganizationChartId,
                OrganizationPositionId= organizationPosition.Id
            };

            var entity = await _processRepository.InsertAsync(process, autoSave: true);

            return true;
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
                if (processCreateOrUpdateDto.PreviousActivityId != null)
                {
                    var previousActivity =await _activityService.GetById(processCreateOrUpdateDto.PreviousActivityId);
                }
                var activity = await _activityService.GetById(processCreateOrUpdateDto.ActivityId);

                var scheme = await _schemeService.GetById(processCreateOrUpdateDto.SchemeId);

                var organizationPosition =await _organizationPositionService.GetByPersonId(processCreateOrUpdateDto.PersonId);

                var organizationChart = await _organizationChartService.GetById(processCreateOrUpdateDto.OrganizationChartId);
                var createdOrganizationChartId = await _organizationChartService.GetById(processCreateOrUpdateDto.CreatedOrganizationChartId);
                if (processCreateOrUpdateDto.PreviousOrganizationChartId is not null)
                {
                    var previousOrganizationChartId = await _organizationChartService.GetById(processCreateOrUpdateDto.PreviousOrganizationChartId);
                }
              
            }

            return process;
        }

    }
}
