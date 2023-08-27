using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using WorkFlowManagement.Domain.Shared.Localization;

namespace WorkFlowManagement.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class WorkFlowManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<WorkFlowManagementResource>("en");
            });
        }
    }
}
