﻿#region NS
using Volo.Abp;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using UserService.Host.Infrastructures.Middlewares;
using UserService.Host.Infrastructures.Extensions;
using IFG.Core.IOC;
using IFG.Core.Caching.Redis;
using IFG.Core.Caching;
using WorkingWithMongoDB.WebAPI.Services;
using UserService.Host.Infrastructures.Hangfire.Abstract;
using UserService.Host.Infrastructures.Hangfire.Concrete;
using UserManagement.Application.UserManagement.Implementations;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using IFG.Core.Caching.Redis.Provider;
#endregion

namespace UserService.Host
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
            services.AddApplication<UserServiceHostModule>();
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
            services.AddSingleton<UserMongoService>();
            services.AddSingleton<IRolePermissionJob, RolePermissionJob>();
            
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            //app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GrpcUserService>();
            });
            app.UseRouting();
            app.InitializeApplication();
        }
    }
}
