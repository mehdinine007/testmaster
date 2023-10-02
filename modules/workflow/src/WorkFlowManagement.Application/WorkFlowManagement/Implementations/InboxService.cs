using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
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

namespace WorkFlowManagement.Application.WorkFlowManagement.Implementations
{
    public class InboxService : ApplicationService, IInboxService
    {

        private readonly IRepository<Inbox, int> _inboxRepository;
        private readonly IRepository<OrganizationChart, int> _organizationChartRepository;
        private readonly IRepository<OrganizationPosition, int> _organizationPositionRepository;
        private readonly ICommonAppService _commonAppService;
        private readonly IOrganizationPositionService _organizationPositionService;

        public InboxService(IRepository<Inbox, int> inboxRepository, IRepository<OrganizationChart, int> organizationChartRepository, IRepository<OrganizationPosition, int> organizationPositionRepository, ICommonAppService commonAppService
            , IOrganizationPositionService organizationPositionService)
        {
            _inboxRepository = inboxRepository;
            _organizationChartRepository = organizationChartRepository;
            _organizationPositionRepository = organizationPositionRepository;
            _commonAppService = commonAppService;
            _organizationPositionService = organizationPositionService;
        }

        public async Task<InboxDto> Add(InboxCreateOrUpdateDto inboxCreateOrUpdateDto)
        {
            await Validation(null, inboxCreateOrUpdateDto);
            var inbox = ObjectMapper.Map<InboxCreateOrUpdateDto, Inbox>(inboxCreateOrUpdateDto);
            var entity = await _inboxRepository.InsertAsync(inbox, autoSave: true);
            return ObjectMapper.Map<Inbox, InboxDto>(entity);
        }

        public async Task<bool> Delete(int id)
        {
            var inbox = await Validation(id, null);
            await _inboxRepository.DeleteAsync(id);
            return true;
        }

        public async Task<InboxDto> GetById(int id)
        {
            var inbox = await Validation(id, null);
            var inboxDto = ObjectMapper.Map<Inbox, InboxDto>(inbox);
            return inboxDto;
        }
        public async Task<InboxDto> GetActiveInboxByProcessId(Guid processId)
        {
            var inboxQuery = (await _inboxRepository.GetQueryableAsync()).OrderByDescending(x => x.Id);
            var inbox = inboxQuery.FirstOrDefault(x => x.ProcessId == processId && x.Status == InboxStatusEnum.Active);
            var inboxDto = ObjectMapper.Map<Inbox, InboxDto>(inbox);
            return inboxDto;
        }

        public async Task<List<InboxDto>> GetList()
        {
            var inbox = (await _inboxRepository.GetQueryableAsync()).ToList();
            var inboxDto = ObjectMapper.Map<List<Inbox>, List<InboxDto>>(inbox);
            return inboxDto;
        }

        public async Task<InboxDto> Update(InboxCreateOrUpdateDto inboxCreateOrUpdateDto)
        {
            var inbox = await Validation(inboxCreateOrUpdateDto.Id, inboxCreateOrUpdateDto);
            inbox.State = inboxCreateOrUpdateDto.State;
            inbox.Status = inboxCreateOrUpdateDto.Status;
            inbox.ReferenceDescription = inboxCreateOrUpdateDto.ReferenceDescription;
            var entity = await _inboxRepository.UpdateAsync(inbox);
            return ObjectMapper.Map<Inbox, InboxDto>(entity);
        }

        public async Task Activate(int? inboxId)
        {
            var inbox = await Validation(inboxId, null);
            inbox.Status = InboxStatusEnum.Active;
            var entity = await _inboxRepository.UpdateAsync(inbox);
        }
        public async Task DeActivate(int? inboxId)
        {
            var inbox = await Validation(inboxId, null);
            inbox.Status = InboxStatusEnum.Posted;
            var entity = await _inboxRepository.UpdateAsync(inbox);
        }

        private async Task<Inbox> Validation(int? id, InboxCreateOrUpdateDto inboxCreateOrUpdateDto)
        {
            var inbox = new Inbox();
            var inboxQuery = (await _inboxRepository.GetQueryableAsync());
            if (id != null)
            {
                inbox = inboxQuery.FirstOrDefault(x => x.Id == id);
                if (inbox is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.InboxNotFound, WorkFlowConstant.InboxNotFoundId);
                }
            }
            if (inboxCreateOrUpdateDto != null)
            {
                var organizationChartQuery = await _organizationChartRepository.GetQueryableAsync();

                var organizationChart = organizationChartQuery.FirstOrDefault(x => x.Id == inboxCreateOrUpdateDto.OrganizationChartId);
                if (organizationChart is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationChartNotFound, WorkFlowConstant.OrganizationChartNotFoundId);
                }
                var organizationPositionQuery = await _organizationPositionRepository.GetQueryableAsync();
                var organizationPosition = organizationPositionQuery.FirstOrDefault(x => x.Id == inboxCreateOrUpdateDto.OrganizationPositionId);
                if (organizationPosition is null)
                {
                    throw new UserFriendlyException(WorkFlowConstant.OrganizationPositionNotFound, WorkFlowConstant.OrganizationPositionNotFoundId);
                }
                if (inboxCreateOrUpdateDto.ParentId is not null)
                {
                    var parent = inboxQuery.FirstOrDefault(x => x.Id == inboxCreateOrUpdateDto.ParentId);
                    if (parent is null)
                    {
                        throw new UserFriendlyException(WorkFlowConstant.InboxParentNotFound, WorkFlowConstant.InboxParentNotFoundId);
                    }
                }
            }
            return inbox;
        }

        public async Task<List<InboxDto>> GetInbox()
        {
            var currentUserId = _commonAppService.GetUserId();
            var organizationPosition =await _organizationPositionService.GetByPersonId(currentUserId);
            var inboxQuery =(await _inboxRepository.GetQueryableAsync()).Include(x=>x.Process).Include(x=>x.Person);
            var inbox = inboxQuery.Where(x => x.OrganizationChartId == organizationPosition.OrganizationChartId && x.Status == InboxStatusEnum.Active && x.PersonId== currentUserId).ToList();
            return ObjectMapper.Map<List<Inbox>, List<InboxDto>>(inbox);
        }

       

    }
}
