using Abp.Application.Services;
using Abp.Application.Services.Dto;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.UserDto;

namespace UserManagement.Application.UserManagement.Implementations;

public class UserAppService : ApplicationService, IUserAppService
{
    public Task Activate(EntityDto<long> user)
    {
        throw new NotImplementedException();
    }

    public Task ChangePassword(ChangePasswordDto forgetPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> CheckUserStatus(CreateUserDto input)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> CreateAsync(CreateUserDto input)
    {
        throw new NotImplementedException();
    }

    public Task DeActivate(EntityDto<long> user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(EntityDto<long> input)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetLoginInfromationuserFromCache(string Username)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserProfile()
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateAsync(UserDto input)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserProfile(UserDto inputUser)
    {
        throw new NotImplementedException();
    }
}
