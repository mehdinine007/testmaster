using WorkFlowManagement.Application.Contracts;
using WorkFlowManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using GatewayManagement.Application.Contracts;

namespace WorkFlowManagement.Application
{
    [DependsOn(
        typeof(WorkFlowManagementDomainModule),
        typeof(WorkFlowManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class WorkFlowManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<WorkFlowManagementApplicationAutoMapperProfile>(validate: true);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }
    }
}

