using OrderManagement.Domain.Shared;
using OrderManagement.Localization;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrderManagement.Domain
{
    /* This module directly depends on EF Core by its design.
     * In this way, we can directly use EF Core async LINQ extension methods.
     */
    [DependsOn(
        typeof(OrderManagementDomainSharedModule),
        typeof(AbpEntityFrameworkCoreModule) 
        )]
    public class OrderManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrderManagementDomainModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Get<OrderManagementResource>().AddVirtualJson("/OrderManagement/Localization/Domain");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("OrderManagement", typeof(OrderManagementResource));
            });
        }
    }
}
