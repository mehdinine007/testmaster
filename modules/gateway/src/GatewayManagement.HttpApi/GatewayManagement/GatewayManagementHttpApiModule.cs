using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using GatewayManagement.Application.Contracts;

namespace GatewayManagement.HttpApi
{
    [DependsOn(
        typeof(GatewayManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class GatewayManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(GatewayManagementHttpApiModule).Assembly);
            });
        }
    }
}
