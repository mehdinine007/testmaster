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
using CompanyManagement.Domain.Shared;
using CompanyManagement.Localization;

namespace CompanyManagement.Domain.CompanyManagement
{
    [DependsOn(
       typeof(CompanyManagementDomainSharedModule), 
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
                options.Resources.Get<CompanyManagementResource>().AddVirtualJson("/CompanyManagement/Localization/Domain");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("CompanyManagement", typeof(CompanyManagementResource));
            });
        }

    }
}
