#region NS
using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using BankService.Host.Infrastructures.Middlewares;
using BankService.Host.Infrastructures.Extensions;
using IFG.Core.IOC;
using IFG.Core.Caching.Redis;
using IFG.Core.Caching;
using CompanyManagement.Application.CompanyManagement.Implementations;
using OrderManagement.Application.CompanyManagement.GrpcServer;
#endregion


namespace BankService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            services.AddApplication<BankServiceHostModule>();
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
            services.AddGrpc();
            services.AddControllers();
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcCompanyService>();
            });
            app.InitializeApplication();

        }
    }
}
