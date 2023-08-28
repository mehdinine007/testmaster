using Microsoft.Extensions.DependencyInjection;
using WorkFlowManagement.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace WorkFlowManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(WorkFlowManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class WorkFlowManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<WorkFlowManagementDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities:true);
            });
        }
    }
}