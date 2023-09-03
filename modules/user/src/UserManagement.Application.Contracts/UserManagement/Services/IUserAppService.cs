using Abp.Application.Services;
using Abp.Application.Services.Dto;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.UserDto;

namespace UserManagement.Application.Contracts.UserManagement.Services;

public interface IUserAppService : IApplicationService
{
    Task<UserDto> CheckUserStatus(CreateUserDto input);

    Task<User> GetLoginInfromationuserFromCache(string Username);

    Task<UserDto> CreateAsync(CreateUserDto input);

    Task<UserDto> GetUserProfile();

    Task UpdateUserProfile(UserDto inputUser);

    Task ChangePassword(ChangePasswordDto forgetPasswordDto);

    Task<UserDto> UpdateAsync(UserDto input);

    Task DeleteAsync(EntityDto<long> input);

    Task Activate(EntityDto<long> user);

    Task DeActivate(EntityDto<long> user);

    //Task<ListResultDto<RoleDto>> GetRoles();
}
