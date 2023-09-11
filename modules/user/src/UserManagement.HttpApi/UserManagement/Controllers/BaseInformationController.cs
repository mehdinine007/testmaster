using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.UserManagement.Bases;

namespace UserManagement.HttpApi.UserManagement.Controllers
{
    [DisableAuditing]
    [RemoteService]
    [Route("api/services/app/BaseInformationController/[action]")]
    //[UserAuthorization]
    public class BaseInformationController : Controller , IBaseInformationService
    {
        private readonly IBaseInformationService _baseInformationService;
        public BaseInformationController(IBaseInformationService baseInformationService)
        {
            _baseInformationService = baseInformationService;
        }


        [HttpPost]
        public async Task<bool> CheckWhiteListAsync(WhiteListEnumType whiteListEnumType, string nationalCode = "")
        {
            await _baseInformationService.CheckWhiteListAsync(whiteListEnumType, nationalCode);
            return true;
        }

    }
}
