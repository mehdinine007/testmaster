using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using Esale.Share.Authorize;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/InboxService/[action]")]
    //[UserAuthorization]
    public class InboxController: Controller
    {

        private readonly IInboxService _inboxService;
        public InboxController(IInboxService inboxService)
        => _inboxService = inboxService;
        [HttpPost]
        public Task<InboxDto> Add(InboxCreateOrUpdateDto inboxCreateOrUpdateDto)
        =>_inboxService.Add(inboxCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_inboxService.Delete(id);
        [HttpGet]
        public Task<InboxDto> GetById(int id)
        =>_inboxService.GetById(id);
        [HttpGet]
        public Task<List<InboxDto>> GetList()
        =>_inboxService.GetList();
        [HttpPut]
        public Task<InboxDto> Update(InboxCreateOrUpdateDto inboxCreateOrUpdateDto)
        =>_inboxService.Update(inboxCreateOrUpdateDto);
        [UserAuthorization]
        [HttpGet]
        public Task<List<InboxDto>> GetActiveList()
       => _inboxService.GetActiveList();

        [UserAuthorization]
        [HttpGet]
        public Task<List<InboxDto>> GetPostedList()
       => _inboxService.GetPostedList();


    }
}
