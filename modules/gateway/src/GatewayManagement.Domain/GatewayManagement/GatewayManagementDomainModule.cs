using GatewayManagement.Domain.Shared;
using GatewayManagement.Domain.Shared.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace GatewayManagement.Domain;
/* This module directly depends on EF Core by its design.
 * In this way, we can directly use EF Core async LINQ extension methods.
 */
[DependsOn(
    typeof(GatewayManagementDomainSharedModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class GatewayManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<GatewayManagementDomainModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<GatewayManagementResource>().AddVirtualJson("/GatewayManagement/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("GatewayManagement", typeof(GatewayManagementResource));
        });
    }
}