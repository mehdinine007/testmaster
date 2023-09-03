using MongoDB.Driver;
using UserManagement.Domain.Authorization.Users;
using Volo.Abp.MongoDB;

namespace Usermanagement.MongoDb;

public class UserManagementMongoDbContext : AbpMongoDbContext
{

    public IMongoCollection<User> User { get; set; }

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        modelBuilder.ConfiureUserManagementMongoDb();
        base.CreateModel(modelBuilder);
    }
}
