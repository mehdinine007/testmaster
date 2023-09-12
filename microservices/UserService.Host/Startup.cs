#region NS
using Volo.Abp;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using UserService.Host.Infrastructures.Middlewares;
using UserService.Host.Infrastructures.Extensions;
using Esale.Core.IOC;
using Esale.Core.Caching.Redis;
using Esale.Core.Caching;
using OrderManagement.Application.OrderManagement.Implementations;
using WorkingWithMongoDB.WebAPI.Services;
using UserService.Host.Infrastructures.Hangfire.Abstract;
using UserService.Host.Infrastructures.Hangfire.Concrete;
#endregion


namespace UserService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            services.AddApplication<UserServiceHostModule>();
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
            services.AddSingleton<GreeterService>();
            services.AddControllers();
            services.AddSingleton<UserMongoService>();
            services.AddSingleton<IRolePermissionJob, RolePermissionJob>();
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseMiddleware<JwtMiddleware>();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            //app.UseRouting();
            app.InitializeApplication();

        }
    }
}
