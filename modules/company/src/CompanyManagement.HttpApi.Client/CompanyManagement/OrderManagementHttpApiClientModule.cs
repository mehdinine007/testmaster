using CompanyManagement.Application.Contracts.CompanyManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace CompanyManagement
{
    [DependsOn(
        typeof(CompanyManagementApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class CompanyManagementHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "CompanyManagement";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(CompanyManagementApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
