using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using CompanyManagement.Application.Contracts.CompanyManagement;

namespace CompanyManagement.HttpApi;

[DependsOn(
    typeof(CompanyManagementApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class BankManagementHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(BankManagementHttpApiModule).Assembly);
        });
    }
}
