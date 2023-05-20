using Microsoft.Extensions.DependencyInjection;
using PaymentManagement.Domain;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PaymentManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(PaymentManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class PaymentManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PaymentManagementDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities:true);
            });
        }
    }
}