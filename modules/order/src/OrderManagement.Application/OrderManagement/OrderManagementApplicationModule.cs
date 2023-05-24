using System;
using Microsoft.Extensions.Caching.Distributed;
using OrderManagement.Application.Contracts;
using OrderManagement.Domain;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using AutoMapper.Internal;
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
        //context.Services.AddScoped<IBaseInformationService, BaseInformationService>();

        //Configure<AbpDistributedCacheOptions>(options =>
        //{
        //    options.GlobalCacheEntryOptions = new DistributedCacheEntryOptions()
        //    {
        //        AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(20)) //20 mins default
        //    };
        //    options.KeyPrefix = "MyApp1";
        //    //options.CacheConfigurators.Add(x =>
        //    //{
        //    //    x.
        //    //});
        //});
    }
}
