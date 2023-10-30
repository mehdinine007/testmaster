using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using AdminPanelManagement.Domain.Shared.Localization;

namespace AdminPanelManagement.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class AdminPanelManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AdminPanelManagementResource>("en");
            });
        }
    }
}
