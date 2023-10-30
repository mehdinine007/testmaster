

using ReportManagement.Domain.Shared;
using ReportManagement.Domain.Shared.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;


namespace ReportManagement.Application.Contracts
{
    [DependsOn(
        typeof(ReportManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class ReportManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<ReportManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<ReportManagementResource>()
                    .AddVirtualJson("/ReportManagement/Localization/ApplicationContracts");
            });
        }
    }
}
