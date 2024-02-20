#region NS
using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OldCarService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using OldCarService.Host.Infrastructures.Middlewares;
using OldCarService.Host.Infrastructures.Extensions;
using IFG.Core.IOC;
using IFG.Core.Caching.Redis;
using IFG.Core.Caching;
using CompanyManagement.Application.CompanyManagement.Implementations;
using OrderManagement.Application.CompanyManagement.GrpcServer;
using System.Linq;
using IFG.Core.Caching.Redis.Provider;
using Microsoft.Extensions.Configuration;

#endregion


namespace OldCarService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            var redisCacheSection = configurations.GetSection("RedisCache");
            var config = redisCacheSection.Get<RedisConfig>();
            var connectionString = "";
            if (config.Password.IsNullOrEmpty())
                connectionString = $"{config.Url}:{config.Port}";
            else
                connectionString = $"{config.Url}:{config.Port},password={config.Password}";

            services.AddApplication<OldCarServiceHostModule>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString;
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
            services.AddGrpc();
            services.AddControllers();
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcCompanyService>();
            });
            app.InitializeApplication();

        }
    }
}
