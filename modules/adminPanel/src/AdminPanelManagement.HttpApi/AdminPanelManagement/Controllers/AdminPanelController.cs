using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelManagement.HttpApi.AdminPanelManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/ActivityService/[action]")]
    //[UserAuthorization]
    public class AdminPanelController: Controller
    {

       // private readonly IActivityService _activityService;
        public AdminPanelController()
        { } 
        //[HttpPost]
        //public Task<ActivityDto> Add(ActivityCreateOrUpdateDto activityCreateOrUpdateDto)
        //=>_activityService.Add(activityCreateOrUpdateDto);
       
    }
}
