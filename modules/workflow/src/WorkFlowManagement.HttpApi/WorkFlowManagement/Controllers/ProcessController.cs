using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;
using Esale.Share.Authorize;
using WorkFlowManagement.Domain.Shared.WorkFlowManagement.Enums;
using WorkFlowManagement.Application.WorkFlowManagement.Implementations;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ProcessService/[action]")]

    public class ProcessController : Controller
    {
        private readonly IProcessService _processService;
        public ProcessController(IProcessService processService)
        => _processService = processService;


        [HttpGet]
        public async Task<ProcessDto> StartProcess(int schemeId)
        => await _processService.StartProcess(schemeId);


        [HttpPost]
        public async Task<ProcessDto> Execute(ExecuteQueryDto executeQueryDto)
        => await _processService.Execute(executeQueryDto);

        [HttpGet]
        public Task<List<InboxDto>> GetOutBox()
      => _processService.GetOutBox();
    }
}
