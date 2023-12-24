using Microsoft.Extensions.DependencyInjection;
using CompanyManagement.Domain;
using CompanyManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using CompanyManagement.EfCore.CompanyManagement.MongoDb;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    [DependsOn(
       typeof(CompanyManagementDomainModule),
       typeof(AbpEntityFrameworkCoreModule)
   )]
    public class CompanyManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<CompanyManagementMongoDbContext>(x => x.AddDefaultRepositories(includeAllEntities: true));
            context.Services.AddAbpDbContext<CompanyManagementDbContext>(options =>
            {
                //options.AddDefaultRepositories();
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            context.Services.AddAbpDbContext<OrderManagementDbContext>(options =>
            {
                //options.AddDefaultRepositories();
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
