using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using GatewayManagement.Application.Contracts;

namespace GatewayManagement.HttpApi
{
    [DependsOn(
        typeof(WorkFlowManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class WorkFlowManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(WorkFlowManagementHttpApiModule).Assembly);
            });
        }
    }
}
