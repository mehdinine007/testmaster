#region NS
using MongoDB.Bson;
using UserManagement.Application.Contracts.Models;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization.Users;
using Volo.Abp.Application.Services;
#endregion


namespace UserManagement.Application.Contracts.UserManagement.Services;

public interface IUserAppService : IApplicationService
{
    Task<User> GetLoginInfromationuserFromCache(string Username);

    Task<bool> AddRole(ObjectId userid, List<string> roleCode);
    Task<UserDto> CreateAsync(CreateUserDto input);
    Task<UserDto> GetUserProfile();
    Task<bool> ForgotPassword(ForgetPasswordDto forgetPasswordDto);
    Task<bool> ChangePassword(ChangePasswordDto input);

    Task UpsertUserIntoSqlServer(UserSQL input);
}

