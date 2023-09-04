using Abp.Authorization;
using MongoDB.Driver;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.MongoDB;

namespace Usermanagement.MongoDb;

public class UserManagementMongoDbContext : AbpMongoDbContext
{

    public IMongoCollection<User> User { get; set; }
    public IMongoCollection<PermissionDefinition> PermissionDefinition => Collection<PermissionDefinition>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        modelBuilder.ConfiureUserManagementMongoDb();
        base.CreateModel(modelBuilder);
    }
}
