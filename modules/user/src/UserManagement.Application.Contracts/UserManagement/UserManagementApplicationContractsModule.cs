using OrderManagement.Domain.Shared;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace UserManagement.Application.Contracts;

[DependsOn(
    typeof(UserManagementDomainSharedModule),
    typeof(AbpDddApplicationModule)
    )]
public class UserManagementApplicationContractsModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<UserManagementApplicationContractsModule>();
        });
    }
}