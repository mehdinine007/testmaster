using Microsoft.Extensions.DependencyInjection;
using ReportManagement.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;


namespace ReportManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(ReportManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ReportManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ReportManagementDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities:true);
            });
        }
    }
}