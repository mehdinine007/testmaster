using AdminPanelManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace AdminPanelManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AdminPanelManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AdminPanelManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AdminPanelManagementDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities:true);
            });
        }
    }
}