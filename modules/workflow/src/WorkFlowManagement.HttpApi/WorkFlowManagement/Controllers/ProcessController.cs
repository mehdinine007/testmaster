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

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ProcessService/[action]")]

    public class ProcessController: Controller
    {
        private readonly IProcessService _processService;
        public ProcessController(IProcessService processService)
        => _processService = processService;
        [HttpPost]
        public Task<ProcessDto> Add(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        =>_processService.Add(processCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(Guid id)
        =>_processService.Delete(id);
        [HttpGet]
        public Task<ProcessDto> GetById(Guid id)
       =>_processService.GetById(id);
        [HttpGet]
        public Task<List<ProcessDto>> GetList()
       =>_processService.GetList();
        [HttpPut]
        public Task<ProcessDto> Update(ProcessCreateOrUpdateDto processCreateOrUpdateDto)
        =>_processService.Update(processCreateOrUpdateDto);


     
        [HttpGet]
        public Task<bool> StartProcess(int schemeId)
        => _processService.StartProcess(schemeId);
    }
}
