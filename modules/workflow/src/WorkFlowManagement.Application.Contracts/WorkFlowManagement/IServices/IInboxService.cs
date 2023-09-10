using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices
{
    public interface IInboxService: IApplicationService
    {

        Task<InboxDto> GetById(int id);
        Task<InboxDto> Add(InboxCreateOrUpdateDto inboxCreateOrUpdateDto);
        Task<InboxDto> Update(InboxCreateOrUpdateDto inboxCreateOrUpdateDto);
        Task<List<InboxDto>> GetList();
        Task<bool> Delete(int id);
        Task<InboxDto> GetActiveInboxByProcessId(Guid processId);
        Task Activate(int? inboxId);
        Task DeActivate(int? inboxId);
        Task<List<InboxDto>> GetActiveList();
    }
}
