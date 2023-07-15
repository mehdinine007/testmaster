using GatewayManagement.Application.Contracts;
using GatewayManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace GatewayManagement.Application
{
    [DependsOn(
        typeof(GatewayManagementDomainModule),
        typeof(GatewayManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class GatewayManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<GatewayManagementApplicationAutoMapperProfile>(validate: true);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }
    }
}

