using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Contracts.Services;
using OrderManagement.Application.OrderManagement.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace OrderManagement.Application.CompanyManagement
{
    public class CompanyManagementApplicationModule: AbpModule
    {
        public static IConfiguration StaticConfig { get; private set; }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CompanyManagementApplicationAutoMapperProfile>();
            });
            StaticConfig = context.Services.GetConfiguration();
            if (StaticConfig.GetValue<bool?>("UseGrpcPaymentService") ?? false)
                context.Services.AddScoped<IIpgServiceProvider, IpgGrpcServiceProvider>();
        }
    }
}
