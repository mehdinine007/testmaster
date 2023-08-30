

using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using WorkFlowManagement.Domain.Shared;
using WorkFlowManagement.Domain.Shared.Localization;

namespace GatewayManagement.Application.Contracts
{
    [DependsOn(
        typeof(WorkFlowManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class WorkFlowManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<WorkFlowManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<WorkFlowManagementResource>()
                    .AddVirtualJson("/WorkFlowManagement/Localization/ApplicationContracts");
            });
        }
    }
}
