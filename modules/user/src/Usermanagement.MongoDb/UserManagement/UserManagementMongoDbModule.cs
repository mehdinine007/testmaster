using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Usermanagement.MongoDb;

[DependsOn(typeof(AbpMongoDbModule))]
public class UserManagementMongoDbModule :AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}
