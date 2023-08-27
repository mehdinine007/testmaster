using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using WorkFlowManagement.Domain.Shared;
using WorkFlowManagement.Domain.Shared.Localization;

namespace WorkFlowManagement.Domain;
/* This module directly depends on EF Core by its design.
 * In this way, we can directly use EF Core async LINQ extension methods.
 */
[DependsOn(
    typeof(WorkFlowManagementDomainSharedModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class WorkFlowManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<WorkFlowManagementDomainModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<WorkFlowManagementResource>().AddVirtualJson("/WorkFlowManagement/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("GatewayManagement", typeof(WorkFlowManagementResource));
        });
    }
}