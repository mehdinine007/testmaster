using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Contracts;

namespace OrderManagement.HttpApi;

[DependsOn(
    typeof(OrderManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class OrderManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(OrderManagementHttpApiModule).Assembly);
        });
    }
}
