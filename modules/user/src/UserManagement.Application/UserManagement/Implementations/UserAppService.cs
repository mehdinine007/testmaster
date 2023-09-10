using MongoDB.Driver;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using Volo.Abp.Application.Services;
using WorkingWithMongoDB.WebAPI.Services;

namespace UserManagement.Application.UserManagement.Implementations;

public class UserAppService : ApplicationService, IUserAppService
{
    private readonly UserMongoService _userMongoService;
    public UserAppService(UserMongoService userMongoService)
    {
        _userMongoService = userMongoService;
    }

    public async Task<User> GetLoginInfromationuserFromCache(string Username)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        object UserFromCache = null;

        var user = (await _userMongoService
                .GetUserCollection())
                .Find(x => x.NormalizedUserName == Username.ToUpper()
                    && x.IsDeleted == false)
                .Project(x =>
                    new User
                    {
                        TempUID = x.UID,
                        UserName = x.UserName,
                        Password = x.Password,
                        IsActive = x.IsActive,
                        RolesM = x.RolesM,
                        NormalizedUserName = x.NormalizedUserName
                    }
                    )
                .FirstOrDefault();
        if (user != null)
        {
            user.UID = new Guid(user.TempUID);
        }
        return user;
    }
}
