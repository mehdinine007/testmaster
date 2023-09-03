using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.IServices;
using WorkFlowManagement.Application.WorkFlowManagement.Implementations;
using WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos;

namespace WorkFlowManagement.HttpApi.WorkFlowManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ActivityRoleService/[action]")]
    //[UserAuthorization]
    public class ActivityRoleController: Controller
    {
        private readonly IActivityRoleService _activityRoleService;
        public ActivityRoleController(IActivityRoleService activityRoleService)
        => _activityRoleService = activityRoleService;
        [HttpPost]
        public Task<ActivityRoleDto> Add(ActivityRoleCreateOrUpdate activityRoleCreateOrUpdate)
        =>_activityRoleService.Add(activityRoleCreateOrUpdate);
        [HttpDelete]
        public Task<bool> Delete(int id)
       =>_activityRoleService.Delete(id);
        [HttpGet]
        public Task<ActivityRoleDto> GetById(int id)
       =>_activityRoleService.GetById(id);
        [HttpGet]
        public Task<List<ActivityRoleDto>> GetList()
        =>_activityRoleService.GetList();
        [HttpPut]
        public Task<ActivityRoleDto> Update(ActivityRoleCreateOrUpdate activityRoleCreateOrUpdate)
        =>_activityRoleService.Update(activityRoleCreateOrUpdate);
    }
}
