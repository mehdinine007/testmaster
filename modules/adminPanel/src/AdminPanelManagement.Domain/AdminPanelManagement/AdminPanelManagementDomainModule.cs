using AdminPanelManagement.Domain.Shared.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using AdminPanelManagement.Domain.Shared;

namespace AdminPanelManagement.Domain;
/* This module directly depends on EF Core by its design.
 * In this way, we can directly use EF Core async LINQ extension methods.
 */
[DependsOn(
    typeof(AdminPanelManagementDomainSharedModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AdminPanelManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AdminPanelManagementDomainModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<AdminPanelManagementResource>().AddVirtualJson("/AdminPanelManagement/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("AdminPanelManagement", typeof(AdminPanelManagementResource));
        });
    }
}