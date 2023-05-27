using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Threading;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using OrderService.Host.Infrastructures.Middlewares;
using OrderService.Host.Infrastructures.Extensions;
using OrderService.Host.Infrastructures.Hangfire;
using OrderService.Host.Infrastructures.Hangfire.Abstract;
using OrderService.Host.Infrastructures.Hangfire.Concrete;
using Esale.Core.IOC;

namespace OrderService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            services.AddApplication<OrderServiceHostModule>();
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
            services.AddSingleton<ICapacityControlJob, CapacityControlJob>();
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
