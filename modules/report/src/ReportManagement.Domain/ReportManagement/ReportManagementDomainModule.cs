using ReportManagement.Domain.Shared;
using ReportManagement.Domain.Shared.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ReportManagement.Domain;
/* This module directly depends on EF Core by its design.
 * In this way, we can directly use EF Core async LINQ extension methods.
 */
[DependsOn(
    typeof(ReportManagementDomainSharedModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class ReportManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ReportManagementDomainModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<ReportManagementResource>().AddVirtualJson("/ReportManagement/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("ReportManagement", typeof(ReportManagementResource));
        });
    }
}