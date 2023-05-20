using PaymentManagement.Domain.Shared;
using PaymentManagement.Domain.Shared.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace PaymentManagement.Domain;
/* This module directly depends on EF Core by its design.
 * In this way, we can directly use EF Core async LINQ extension methods.
 */
[DependsOn(
    typeof(PaymentManagementDomainSharedModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class PaymentManagementDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<PaymentManagementDomainModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Get<PaymentManagementResource>().AddVirtualJson("/PaymentManagement/Localization/Domain");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("PaymentManagement", typeof(PaymentManagementResource));
        });
    }
}