using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using GatewayManagement.Domain.Shared.Localization;

namespace GatewayManagement.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class GatewayManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<GatewayManagementResource>("en");
            });
        }
    }
}
