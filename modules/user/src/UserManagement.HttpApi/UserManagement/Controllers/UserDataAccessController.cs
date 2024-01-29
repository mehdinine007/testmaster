using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Contracts.UserManagement.Models;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.UserManagement.Implementations;
using Volo.Abp;
using Volo.Abp.Auditing;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/UserDataAccessService/[action]")]
    public class UserDataAccessController:Controller
    {
        private readonly IUserDataAccessService _UserDataAccessService;
        public UserDataAccessController(IUserDataAccessService UserDataAccessService)
        =>_UserDataAccessService = UserDataAccessService;

        [HttpGet]
        public Task<List<UserDataAccessDto>> GetListByNationalcode(string nationalcode)
       =>_UserDataAccessService.GetListByNationalcode(nationalcode);
        [HttpGet]
        public Task<List<UserDataAccessDto>> GetListByUserId(Guid userId)
        =>_UserDataAccessService.GetListByUserId(userId);
    }
}
