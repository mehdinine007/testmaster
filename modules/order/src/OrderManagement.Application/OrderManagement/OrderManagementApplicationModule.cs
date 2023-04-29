using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace OrderManagement
{
    [DependsOn(
        typeof(OrderManagementDomainModule),
        typeof(OrderManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class OrderManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<OrderManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
