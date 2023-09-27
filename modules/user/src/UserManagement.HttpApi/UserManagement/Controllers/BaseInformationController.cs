using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.UserManagement;
using UserManagement.Application.Contracts.Models;
using MongoDB.Bson;
using UserManagement.Application.Contracts.Services;
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/BaseInformationService/[action]")]
    //[UserAuthorization]
    public class BaseInformationController : Controller
    {
        private readonly IBaseInformationService _baseInformationService;
        public BaseInformationController(IBaseInformationService baseInformationService)
        {
            _baseInformationService = baseInformationService;
        }

        [HttpGet]
        public Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string Nationalcode)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public Task<UserGrpcDto> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        public async Task RegistrationValidation(RegistrationValidationDto input)
         => await _baseInformationService.RegistrationValidation(input);
        
        [HttpGet]
        public void RegistrationValidationWithoutCaptcha(RegistrationValidationDto input)
        {
            throw new NotImplementedException();
        }
    }
}
