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

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ActivityService/[action]")]
    //[UserAuthorization]
    public class ActivityController: Controller
    {

        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        => _activityService = activityService;
        [HttpPost]
        public Task<ActivityDto> Add(ActivityCreateOrUpdateDto activityCreateOrUpdateDto)
        =>_activityService.Add(activityCreateOrUpdateDto);
        [HttpDelete]
        public Task<bool> Delete(int id)
       =>_activityService.Delete(id);
        [HttpGet]
        public Task<ActivityDto> GetById(int id)
       =>_activityService.GetById(id);
        [HttpGet]
        public Task<List<ActivityDto>> GetList()
       =>_activityService.GetList();
        [HttpPut]
        public Task<ActivityDto> Update(ActivityCreateOrUpdateDto activityCreateOrUpdateDto)
        =>_activityService.Update(activityCreateOrUpdateDto);
    }
}
