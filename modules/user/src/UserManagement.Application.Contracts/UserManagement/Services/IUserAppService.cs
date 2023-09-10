using UserManagement.Domain.Authorization.Users;
using Volo.Abp.Application.Services;

namespace UserManagement.Application.Contracts.UserManagement.Services;

public interface IUserAppService : IApplicationService

{
    Task<User> GetLoginInfromationuserFromCache(string Username);
}
