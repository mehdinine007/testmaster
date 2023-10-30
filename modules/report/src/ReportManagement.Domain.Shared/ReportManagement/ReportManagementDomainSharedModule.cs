using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using ReportManagement.Domain.Shared.Localization;

namespace ReportManagement.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class ReportManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<ReportManagementResource>("en");
            });
        }
    }
}
