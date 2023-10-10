using GatewayManagement.Application.Contracts;
using ReportManagement.Application.Contracts;
using ReportManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;



namespace ReportManagement.Application
{
    [DependsOn(
        typeof(ReportManagementDomainModule),
        typeof(ReportManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class ReportManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<ReportManagementApplicationAutoMapperProfile>(validate: true);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }
    }
}

