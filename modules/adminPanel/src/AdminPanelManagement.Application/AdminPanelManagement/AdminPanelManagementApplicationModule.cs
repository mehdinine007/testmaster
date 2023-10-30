using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using AdminPanelManagement.Application;
using AdminPanelManagement.Domain;
using AdminPanelManagement.Application.Contracts;

namespace AdminPanelManagement.Application
{
    [DependsOn(
        typeof(AdminPanelManagementDomainModule),
        typeof(AdminPanelManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class AdminPanelManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AdminPanelManagementApplicationAutoMapperProfile>(validate: true);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }
    }
}

