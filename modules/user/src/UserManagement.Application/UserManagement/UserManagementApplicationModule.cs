using UserManagement.Application.Contracts;
using UserManagement.Domain.UserManagement;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace UserManagement.Application;

[DependsOn(
    typeof(UserManagementDomainModule),
    typeof(UserManagementApplicationContractsModule))]
public class UserManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<UserManagementApplicationModule>();
        });
    }
}
