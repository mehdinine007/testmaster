using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Contracts;

namespace CompanyManagement.HttpApi;

[DependsOn(
    typeof(OrderManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class CompanyManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CompanyManagementHttpApiModule).Assembly);
        });
    }
}
