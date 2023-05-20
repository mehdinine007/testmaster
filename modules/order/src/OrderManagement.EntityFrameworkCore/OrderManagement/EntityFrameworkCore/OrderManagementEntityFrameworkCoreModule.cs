using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OrderManagement.EfCore
{
    [DependsOn(
        typeof(OrderManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class OrderManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderManagementDbContext>(options =>
            {
                //options.AddDefaultRepositories();
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}