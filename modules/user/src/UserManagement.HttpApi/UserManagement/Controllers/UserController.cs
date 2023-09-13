using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.Models;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Domain.Authorization.Users;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class UserController : AbpController, IUserAppService
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService)
        => _userAppService = userAppService;


    [HttpPost]
    public async Task<UserDto> CreateAsync(CreateUserDto input)
        => await _userAppService.CreateAsync(input);

    [HttpGet]
    public async Task<User> GetLoginInfromationuserFromCache(string Username)
        => await _userAppService.GetLoginInfromationuserFromCache(Username);

    public Task<UserDto> GetUserProfile()
    {
        throw new NotImplementedException();
    }
}