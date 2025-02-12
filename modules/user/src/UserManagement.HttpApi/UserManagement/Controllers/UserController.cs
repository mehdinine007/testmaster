﻿using Volo.Abp.Auditing;
using Volo.Abp;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Application.Contracts.Models;
using Volo.Abp.AspNetCore.Mvc;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Application.Contracts.UserManagement.Models.User;
using Esale.Share.Authorize;

namespace UserManagement.HttpApi.UserManagement.Controllers;

[DisableAuditing]
[RemoteService]
[Route("api/services/app/[controller]/[action]")]
public class UserController : AbpController
{
    private readonly IUserAppService _userAppService;

    public UserController(IUserAppService userAppService)
        => _userAppService = userAppService;

    [HttpPost]
    public async Task<bool> AddUserRole(UserRoleDto dto)
        => await _userAppService.AddRole(dto.userid, dto.roleCode);

    [HttpPost]
    public async Task<UserDto> CreateAsync(CreateUserDto input)
        => await _userAppService.CreateAsync(input);

    [HttpGet]
    public async Task<User> GetLoginInfromationuserFromCache(string Username)
        => await _userAppService.GetLoginInfromationuserFromCache(Username);

    [HttpGet]
    public async Task<UserDto> GetUserProfile()
        => await _userAppService.GetUserProfile();
    [HttpPost]
    public async Task<bool> UpdateUserProfile(UserDto inputUser)
        => await _userAppService.UpdateUserProfile(inputUser);

    [HttpPost]
    public async Task<bool> ForgotPassword(ForgetPasswordDto forgetPasswordDto)
        => await _userAppService.ForgotPassword(forgetPasswordDto);

    [HttpPost]
    public async Task<bool> ChangePassword(ChangePasswordDto input)
        => await _userAppService.ChangePassword(input);

    [HttpPost]
    public async Task<bool> ChangeMobile(ChangeMobileDto changeMobileDto)
            => await _userAppService.ChangeMobile(changeMobileDto);
}
