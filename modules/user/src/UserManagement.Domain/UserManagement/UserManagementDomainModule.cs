using UserManagement.Domain.Shared;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace UserManagement.Domain.UserManagement;

[DependsOn(typeof(UserManagementDomainSharedModule))]
public class UserManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<UserManagementDomainModule>();
        });
    }
}

public class UserDbConsts
{
    public const string EfConnectionStrinName = "UserManagement";
}
