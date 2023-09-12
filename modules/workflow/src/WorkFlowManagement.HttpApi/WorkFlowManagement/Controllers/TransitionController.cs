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

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/TransitionService/[action]")]
    //[UserAuthorization]
    public class TransitionController: Controller
    {
        private readonly ITransitionService _transitionService;
        public TransitionController(ITransitionService transitionService)
        => _transitionService = transitionService;
        [HttpPost]
        public Task<TransitionDto> Add(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto)
        =>_transitionService.Add(transitionCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
        =>_transitionService.Delete(id);
        [HttpGet]
        public Task<TransitionDto> GetById(int id)
        =>_transitionService.GetById(id);
        [HttpGet]
        public Task<List<TransitionDto>> GetList(int activitySourceId)
        =>_transitionService.GetList(activitySourceId);
        [HttpPut]
        public Task<TransitionDto> Update(TransitionCreateOrUpdateDto transitionCreateOrUpdateDto)
        =>_transitionService.Update(transitionCreateOrUpdateDto);
    }
}
