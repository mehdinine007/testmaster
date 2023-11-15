using MongoDB.Driver;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace UserManagement.EfCore.MongoDb;

[ConnectionStringName("MongoConnection")]
public class UserManagementMongoDbContext : AbpMongoDbContext
{
    public IMongoCollection<PermissionDefinition> PermissionDefinition => Collection<PermissionDefinition>();
    public IMongoCollection<RolePermission> RolePermission => Collection<RolePermission>();
    public IMongoCollection<UserMongo> Customers => Collection<UserMongo>();
    public IMongoCollection<Menu> Menu => Collection<Menu>();


    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);
    }
}
