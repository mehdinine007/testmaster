using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using PaymentManagement.Domain.Shared.Localization;

namespace PaymentManagement.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule)
        )]
    public class PaymentManagementDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<PaymentManagementResource>("en");
            });
        }
    }
}
