using OrderManagement.Domain.Shared;
using OrderManagement.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrderManagement.Domain.CompanyManagement
{
    [DependsOn(
       typeof(OrderManagementDomainSharedModule),
       typeof(AbpEntityFrameworkCoreModule)
       )]
    public class CompanyManagementDomainModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CompanyManagementDomainModule>();
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
