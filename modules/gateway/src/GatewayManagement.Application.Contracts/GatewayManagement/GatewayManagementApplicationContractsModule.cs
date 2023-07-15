using GatewayManagement.Domain.Shared;
using GatewayManagement.Domain.Shared.Localization;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace GatewayManagement.Application.Contracts
{
    [DependsOn(
        typeof(GatewayManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class GatewayManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<GatewayManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<GatewayManagementResource>()
                    .AddVirtualJson("/GatewayManagement/Localization/ApplicationContracts");
            });
        }
    }
}
