using PaymentManagement.Domain.Shared;
using PaymentManagement.Domain.Shared.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace PaymentManagement.Application.Contracts
{
    [DependsOn(
        typeof(PaymentManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class PaymentManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<PaymentManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<PaymentManagementResource>()
                    .AddVirtualJson("/PaymentManagement/Localization/ApplicationContracts");
            });
        }
    }
}
