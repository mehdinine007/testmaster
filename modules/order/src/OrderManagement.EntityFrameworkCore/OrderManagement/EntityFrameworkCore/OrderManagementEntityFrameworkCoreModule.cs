using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace OrderManagement.EntityFrameworkCore
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
                options.AddDefaultRepositories();
            });
        }
    }
}