using PaymentManagement.Application.Contracts;
using PaymentManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace PaymentManagement.Application
{
    [DependsOn(
        typeof(PaymentManagementDomainModule),
        typeof(PaymentManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class PaymentManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<PaymentManagementApplicationAutoMapperProfile>(validate: true);
            });
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            base.OnPreApplicationInitialization(context);
        }
    }
}

