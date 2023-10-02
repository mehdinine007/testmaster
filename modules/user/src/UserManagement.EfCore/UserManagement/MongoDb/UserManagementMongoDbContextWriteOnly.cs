using MongoDB.Driver;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace UserManagement.EfCore.MongoDb;

[ConnectionStringName("MongoWriteConnection")]
public class UserManagementMongoDbContextWriteOnly : AbpMongoDbContext
{
    public IMongoCollection<PermissionDefinitionWrite> PermissionDefinition => Collection<PermissionDefinitionWrite>();
    public IMongoCollection<RolePermissionWrite> RolePermission => Collection<RolePermissionWrite>();
    public IMongoCollection<UserMongoWrite> Customers => Collection<UserMongoWrite>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);
    }
}