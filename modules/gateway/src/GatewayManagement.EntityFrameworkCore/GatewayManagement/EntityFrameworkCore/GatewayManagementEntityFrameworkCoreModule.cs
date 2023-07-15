using Microsoft.Extensions.DependencyInjection;
using GatewayManagement.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace GatewayManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(GatewayManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class GatewayManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GatewayManagementDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities:true);
            });
        }
    }
}