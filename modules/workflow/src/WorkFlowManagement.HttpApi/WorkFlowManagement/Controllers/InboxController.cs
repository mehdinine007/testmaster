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
       
        [HttpGet]
        public Task<List<InboxDto>> GetInbox()
       => _inboxService.GetInbox();
    }
}
