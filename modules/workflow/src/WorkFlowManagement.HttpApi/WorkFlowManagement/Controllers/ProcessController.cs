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

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ProcessService/[action]")]

    public class ProcessController: Controller
    {
        private readonly IProcessService _processService;
        public  ProcessController(IProcessService processService)
        => _processService = processService;
        [HttpPost]
        public async Task<ProcessDto> Add(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        =>await _processService.Add(processCreateOrUpdateDto);
        [HttpDelete]
        public async Task<bool> Delete(Guid id)
        =>await _processService.Delete(id);
        [HttpGet]
        public async  Task<ProcessDto> GetById(Guid id)
       =>await _processService.GetById(id);
        [HttpGet]
        public async Task<List<ProcessDto>> GetList()
       =>await _processService.GetList();
        [HttpPut]
        public async Task<ProcessDto> Update(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        =>await _processService.Update(processCreateOrUpdateDto);


        [UserAuthorization]
        [HttpGet]
        public async  Task<ProcessDto> StartProcess(int schemeId)
        =>await _processService.StartProcess(schemeId);


        [HttpPost]
        public async Task<ProcessDto> Execute(ExecuteQueryDto executeQueryDto)
        => await _processService.Execute(executeQueryDto);
    }
}
