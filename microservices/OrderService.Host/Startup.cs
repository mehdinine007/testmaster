#region NS
using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using OrderService.Host.Infrastructures.Middlewares;
using OrderService.Host.Infrastructures.Extensions;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using OrderService.Host.Infrastructures.Hangfire.Concrete;
using IFG.Core.IOC;
using IFG.Core.Caching.Redis;
using IFG.Core.Caching;
using IFG.Core.Caching.Redis.Provider;
using Microsoft.Extensions.Configuration;
using System.Linq;
#endregion


namespace OrderService.Host
{
    public class Startup
    {
      
        public void ConfigureServices(IServiceCollection services)
        {
            
            var configurations = services.GetConfiguration();
            var redisCacheSection = configurations.GetSection("RedisCache");
            var config = redisCacheSection.Get<RedisConfig>();
            var connectionString = "";
            if (string.IsNullOrEmpty(config.Password))
                connectionString = $"{config.Url}:{config.Port}";
            else
                connectionString = $"{config.Url}:{config.Port},password={config.Password}";


            services.AddApplication<OrderServiceHostModule>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =connectionString;
            });
            if (configurations["IsElkEnabled"] == "1")
            {
                services.ElkNest(configurations, configurations["ElkIndexName"]);
            }
            else
            {
                services.AddScoped<IAuditingStore, AuditingStoreDb>();
            }
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<IRedisCacheManager, RedisCacheManager>();
            services.AddSingleton<ICapacityControlJob, CapacityControlJob>();
            services.AddSingleton<IIpgJob, IpgJob>();
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseMiddleware<JwtMiddleware>();
            app.InitializeApplication();
        }
    }
}
