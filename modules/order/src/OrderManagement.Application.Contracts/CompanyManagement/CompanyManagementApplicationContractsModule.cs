using OrderManagement.Domain.Shared;
using OrderManagement.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace OrderManagement.Application.Contracts.CompanyManagement
{

    [DependsOn(
        typeof(OrderManagementDomainSharedModule),
        typeof(AbpDddApplicationModule)
        )]
    public class CompanyManagementApplicationContractsModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CompanyManagementApplicationContractsModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<OrderManagementResource>()
                    .AddVirtualJson("/OrderManagement/Localization/ApplicationContracts");
            });
        }

    }
}
