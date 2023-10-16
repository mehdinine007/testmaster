#region NS
using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.Services;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.UserManagement.Services;
#endregion

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class UserAppContoller : Controller
{
    private readonly IUserAppService _userAppService;
    public UserAppContoller(IUserAppService userAppService)
    {
        _userAppService = userAppService;
    }

    [HttpPost]
    public async Task<bool> ForgotPassword(ForgetPasswordDto forgetPasswordDto)
    {
        return await _userAppService.ForgotPassword(forgetPasswordDto);
    }

    [HttpPost]
    public async Task<bool> ChangePassword(ChangePasswordDto input)
        => await _userAppService.ChangePassword(input);
}