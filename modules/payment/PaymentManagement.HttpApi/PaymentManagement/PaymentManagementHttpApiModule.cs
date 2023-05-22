using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using PaymentManagement.Application.Contracts;

namespace PaymentManagement.HttpApi
{
    [DependsOn(
        typeof(PaymentManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class PaymentManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(PaymentManagementHttpApiModule).Assembly);
            });
        }
    }
}
