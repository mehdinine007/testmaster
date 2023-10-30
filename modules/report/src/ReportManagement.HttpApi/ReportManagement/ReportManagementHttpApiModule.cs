using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using ReportManagement.Application.Contracts;

namespace ReportManagement.HttpApi
{
    [DependsOn(
        typeof(ReportManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ReportManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(ReportManagementHttpApiModule).Assembly);
            });
        }
    }
}
