

using AdminPanelManagement.Domain.Shared;
using AdminPanelManagement.Domain.Shared.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace AdminPanelManagement.Application.Contracts
{
    [DependsOn(
        typeof(AdminPanelManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class AdminPanelManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AdminPanelManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AdminPanelManagementResource>()
                    .AddVirtualJson("/AdminPanelManagement/Localization/ApplicationContracts");
            });
        }
    }
}
