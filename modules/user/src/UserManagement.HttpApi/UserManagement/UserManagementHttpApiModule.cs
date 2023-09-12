using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Contracts;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace UserManagement.HttpApi.UserManagement;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(UserManagementApplicationContractsModule))]
public class UserManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(UserManagementHttpApiModule).Assembly);
        });
    }
}
