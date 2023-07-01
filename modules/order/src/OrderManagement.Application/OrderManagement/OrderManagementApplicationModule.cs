using OrderManagement.Application.Contracts;
using OrderManagement.Domain;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Implementations;
using Microsoft.Extensions.Configuration;

namespace OrderManagement.Application;
[DependsOn(
    typeof(OrderManagementDomainModule),
    typeof(OrderManagementApplicationContractsModule),
    typeof(AbpAutoMapperModule)
    )]
public class OrderManagementApplicationModule : AbpModule
{
    public static IConfiguration StaticConfig { get; private set; }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<OrderManagementApplicationAutoMapperProfile>();
        });
        StaticConfig = context.Services.GetConfiguration();
        if(StaticConfig.GetValue<bool?>("UseGrpcPaymentService") ?? false)
            context.Services.AddScoped<IIpgServiceProvider, IpgGrpcServiceProvider>();
    }
}
