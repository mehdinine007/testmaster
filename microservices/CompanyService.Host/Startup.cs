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
using Esale.Core.IOC;
using Esale.Core.Caching.Redis;
using Esale.Core.Caching;
using CompanyService.Host;
#endregion


namespace CompanyService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            services.AddApplication<CompanyServiceHostModule>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configurations["RedisCache:ConnectionString"];
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
