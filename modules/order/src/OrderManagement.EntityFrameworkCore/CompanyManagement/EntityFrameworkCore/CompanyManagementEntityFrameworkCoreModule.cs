using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain;
using OrderManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OrderManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    [DependsOn(
       typeof(CompanyManagementDomainModule),
       typeof(AbpEntityFrameworkCoreModule)
   )]
    public class CompanyManagementEntityFrameworkCoreModule: AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CompanyManagementDbContext>(options =>
            {
                //options.AddDefaultRepositories();
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
