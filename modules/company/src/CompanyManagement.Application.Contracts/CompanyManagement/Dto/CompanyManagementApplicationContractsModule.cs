using CompanyManagement.Domain.Shared;
using CompanyManagement.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application;
using Volo.Abp.FluentValidation;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace CompanyManagement.Application.Contracts.CompanyManagement
{

    [DependsOn(
        typeof(CompanyManagementDomainSharedModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpFluentValidationModule)

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
                    .Get<CompanyManagementResource>()
                    .AddVirtualJson("/CompanyManagement/Localization/ApplicationContracts");
            });
        }

    }
}
