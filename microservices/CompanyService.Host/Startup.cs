#region NS
using System;
using Volo.Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Host.Infrastructure.Middlewares;
using Volo.Abp.Auditing;
using CompanyService.Host.Infrastructures.Middlewares;
using CompanyService.Host.Infrastructures.Extensions;
using IFG.Core.IOC;
using IFG.Core.Caching.Redis;
using IFG.Core.Caching;
using CompanyManagement.Application.CompanyManagement.Implementations;
using OrderManagement.Application.CompanyManagement.GrpcServer;
using System.Linq;
using IFG.Core.Caching.Redis.Provider;
using Microsoft.Extensions.Configuration;
#endregion


namespace CompanyService.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configurations = services.GetConfiguration();
            var redisCacheSection = configurations.GetSection("RedisCache");
            var config = redisCacheSection.Get<RedisConfig>();
            var redisContString = "";
            if (config.Password.IsNullOrEmpty())
                redisContString = $"{config.Url}:{config.Port}";
            else
                redisContString = $"{config.Url}:{config.Port},password={config.Password}";

            services.AddApplication<CompanyServiceHostModule>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisContString;
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
            services.AddHealthChecks()
                .AddSqlServer(configurations.GetSection("ConnectionStrings:CompanyManagement").Value)
                .AddRedis(redisContString)
                //.AddMongoDb($"mongodb://{mongoConfig.Host}:{mongoConfig.Port}")
                .AddElasticsearch(configurations.GetSection("ElasticSearch:Url").Value ?? "http://localhost:9200")
                .AddUrlGroup(new Uri($"{configurations.GetSection("Grpc:UserUrl").Value ?? "http://localhost:9100"}/api/services/app/Licence/GetInfo"), httpMethod: HttpMethod.Get, name: "grpc-user",
                configurePrimaryHttpMessageHandler: _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });
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
