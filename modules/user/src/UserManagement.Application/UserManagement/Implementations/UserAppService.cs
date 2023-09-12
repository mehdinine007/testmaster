using MongoDB.Bson;
using MongoDB.Driver;
using UserManagement.Application.Contracts.UserManagement.Constant;
using UserManagement.Application.Contracts.UserManagement.Services;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WorkingWithMongoDB.WebAPI.Services;

namespace UserManagement.Application.UserManagement.Implementations;

public class UserAppService : ApplicationService, IUserAppService
{
    
    private readonly IRolePermissionService _rolePermissionService;
    private readonly IRepository<UserMongo, ObjectId> _userMongoRepository;
    public UserAppService(IRolePermissionService rolePermissionService,
                          IRepository<UserMongo, ObjectId> userMongoRepository
        )
    {

        _rolePermissionService = rolePermissionService;
        _userMongoRepository = userMongoRepository;
    }

    public async Task<bool> AddRole(ObjectId userid, List<string> roleCode)
    {
        var user = (await _userMongoRepository
               .GetQueryableAsync())
               .FirstOrDefault(x => x.Id == userid);

        if (user is null)
            return false;
        foreach (var cod in roleCode)
        {
            if (await _rolePermissionService.ValidationByCode(cod))
                user.Roles.Add(cod);
        }
        await _userMongoRepository.UpdateAsync(user);
        return true;
    }



    public async Task<User> GetLoginInfromationuserFromCache(string Username)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        object UserFromCache = null;

        var user = (await _userMongoRepository
                .GetQueryableAsync())
                .Where(x => x.NormalizedUserName == Username.ToUpper()
                  )
                .Select(x => new User
                {
                    TempUID = x.UID,
                    UserName = x.UserName,
                    Password = x.Password,
                    IsActive = x.IsActive,
                    RolesM = x.Roles,
                    NormalizedUserName = x.NormalizedUserName
                })
                .FirstOrDefault();
        if (user != null)
        {
            user.UID = new Guid(user.TempUID);
        }
        return user;
    }
}
