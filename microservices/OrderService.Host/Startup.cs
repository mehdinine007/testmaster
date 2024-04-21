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
using IFG.Core.Bases;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using System.Net.Http;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
#endregion


namespace OrderService.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            var configurations = services.GetConfiguration();
            var redisCacheSection = configurations.GetSection("RedisCache");
            var config = redisCacheSection.Get<RedisConfig>();
            var redisContString = "";
            if (string.IsNullOrEmpty(config.Password))
                redisContString = $"{config.Url}:{config.Port}";
            else
                redisContString = $"{config.Url}:{config.Port},password={config.Password}";
            var mongoConfig = configurations.GetSection("MongoConfig").Get<MongoConfig>();


            services.AddApplication<OrderServiceHostModule>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration =redisContString;
            });
            if (configurations["IsElkEnabled"] == "1")
            {
                services.ElkNest(configurations, configurations["ElkIndexName"]);
            }
            else
            {
                services.AddScoped<IAuditingStore, AuditingStoreDb>();
            }
            services.AddHealthChecks()
                .AddSqlServer(configurations.GetSection("ConnectionStrings:OrderManagement").Value)
                .AddRedis(redisContString)
                //.AddMongoDb($"mongodb://{mongoConfig.Host}:{mongoConfig.Port}")
                .AddElasticsearch(configurations.GetSection("ElasticSearch:Url").Value ?? "http://localhost:9200")
                .AddUrlGroup(new Uri($"{configurations.GetSection("Esale:GrpcAddress").Value??"http://localhost:9100"}/api/services/app/Licence/GetInfo"), httpMethod: HttpMethod.Get, name: "grpc-user",
                configurePrimaryHttpMessageHandler: _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                })
                .AddUrlGroup(new Uri($"{configurations.GetSection("Company:GrpcAddress").Value ?? "http://localhost:9000"}/api/services/app/Licence/GetInfo"), httpMethod: HttpMethod.Get, name: "grpc-company",
                configurePrimaryHttpMessageHandler: _ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });

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
