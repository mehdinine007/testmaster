using Microsoft.EntityFrameworkCore;
using Nest;
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
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Constants;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
using WorkFlowManagement.Domain.WorkFlowManagement;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
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
        private readonly IActivityRoleService _activityRoleService;
        private readonly IRoleOrganizationChartService _roleOrganizationChartService;
        private readonly IInboxService _inboxService;
        public ProcessService(IRepository<Process
            , Guid> processRepository, ISchemeService schemeService
            , IActivityService activityService
            , ICommonAppService commonAppService
            , IOrganizationPositionService organizationPositionService
            , IOrganizationChartService organizationChartService
             , IActivityRoleService activityRoleService
             , IRoleOrganizationChartService roleOrganizationChartService
             , IInboxService inboxService

            )
        {
            _processRepository = processRepository;
            _schemeService = schemeService;
            _activityService = activityService;
            _commonAppService = commonAppService;
            _organizationPositionService = organizationPositionService;
            _organizationChartService = organizationChartService;
            _activityRoleService = activityRoleService;
            _roleOrganizationChartService = roleOrganizationChartService;
            _inboxService = inboxService;
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
            var processQuery = (await _processRepository.GetQueryableAsync()).Include(x => x.Activity);
            var process = processQuery.FirstOrDefault(x => x.Id == id);
            var processDto = ObjectMapper.Map<Process, ProcessDto>(process);
            return processDto;
        }

        public async Task<List<ProcessDto>> GetList()
        {
            var process = (await _processRepository.GetQueryableAsync()).Include(x => x.Scheme).ToList();
            var processDto = ObjectMapper.Map<List<Process>, List<ProcessDto>>(process);
            return processDto;
        }

        public async Task<ProcessDto> StartProcess(int schemeId)
        {
            var currentUserId = _commonAppService.GetUserId();
            var scheme = await _schemeService.GetById(schemeId);
            var activity = await _activityService.GetBySchemeId(schemeId);
            var organizationPosition = await _organizationPositionService.GetByPersonId(currentUserId);
            var process = new Process()
            {
                SchemeId = schemeId,
                Status = StatusEnum.Initialize,
                State = StateEnum.Start,
                Title = scheme.Title,
                Subject = "",
                Description = "",
                ActivityId = activity.Id,
                PersonId = currentUserId,
                CreatedPersonId = currentUserId,
                OrganizationChartId = organizationPosition.OrganizationChartId,
                CreatedOrganizationChartId = organizationPosition.OrganizationChartId,
                OrganizationPositionId = organizationPosition.Id
            };
            var entity = await _processRepository.InsertAsync(process, autoSave: true);
            await InsertInbox(entity, null);
            return await GetById(entity.Id);
        }

        private async Task InsertInbox(Process process, int? inboxId)
        {
            var createInbox = new InboxCreateOrUpdateDto()
            {
                ProcessId = process.Id,
                PersonId = process.PersonId,
                OrganizationChartId = process.OrganizationChartId,
                OrganizationPositionId = process.OrganizationPositionId,
                State = process.State,
                ReferenceDescription = process.Description,
                ParentId = inboxId,
                Status = InboxStatusEnum.Active
            };

            await _inboxService.Add(createInbox);

            if (inboxId != null)
            {
                _inboxService.DeActivate(inboxId);
            }

        }


    public async Task<ProcessDto> Execute(ExecuteQueryDto executeQueryDto)
    {
        var process = await Validation(executeQueryDto.ProcessId, null);
        var activity = await _activityService.NextAcvtivity(process.ActivityId);
        var activityRole = await _activityRoleService.GetByActivityId(activity.Id);
        var roleOrganizationChart = await _roleOrganizationChartService.GetByRoleId(activityRole.RoleId);
        var organizationPosition = await _organizationPositionService.GetByOrganizationChartId(roleOrganizationChart.OrganizationChartId);
        if (executeQueryDto.Action == StateEnum.Draft)
        {
            process.State = StateEnum.Draft;
        }
        else if (executeQueryDto.Action == StateEnum.Refrence)
        {
            process.Status = StatusEnum.Runing;
            process.State = StateEnum.Refrence;
            process.PreviousActivityId = process.ActivityId;
            process.ActivityId = activity.Id;
            process.PreviousOrganizationChartId = process.OrganizationChartId;
            process.PreviousPersonId = process.PersonId;
            process.PersonId = organizationPosition.PersonId;
        }
        var entity = await _processRepository.UpdateAsync(process, autoSave: true);
        var parent = await _inboxService.GetActiveInboxByProcessId(process.Id);
        await InsertInbox(entity, parent.Id);
        return await GetById(entity.Id);
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
                var previousActivity = await _activityService.GetById(processCreateOrUpdateDto.PreviousActivityId);
            }
            var activity = await _activityService.GetById(processCreateOrUpdateDto.ActivityId);

            var scheme = await _schemeService.GetById(processCreateOrUpdateDto.SchemeId);

            var organizationPosition = await _organizationPositionService.GetByPersonId(processCreateOrUpdateDto.PersonId);

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
