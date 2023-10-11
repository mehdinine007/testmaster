using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using AdminPanelManagement.Application.Contracts;

namespace AdminPanelManagement.HttpApi
{
    [DependsOn(
        typeof(AdminPanelManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AdminPanelManagementHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AdminPanelManagementHttpApiModule).Assembly);
            });
        }
    }
}
