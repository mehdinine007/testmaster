using EasyCaching.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace IFG.Core.Caching
{
    public static class EasyCachingExtention
    {
        public static IServiceCollection EasyCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSection = configuration.GetSection("RedisCache");
            var connectionString = redisCacheSection["ConnectionString"];
            var databaseId = redisCacheSection["DatabaseId"];
            var connectionStringUser = redisCacheSection["ConnectionStringUser"];
            var databaseIdUser = redisCacheSection["DatabaseIdUser"];
            var port = redisCacheSection["Port"]!=null ? int.Parse(redisCacheSection["Port"]):6379 ;
            var password = redisCacheSection["Password"];
            services.AddEasyCaching(options =>
            {
                options.WithJson("myjson");

                // local
                options.UseInMemory("m1");
                // distributed
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(connectionString, port));

                    config.DBConfig.Database = 5;
                    config.SerializerName = "myjson";

                    config.DBConfig.Password = password;
                }, "myredis");
              

                // combine local and distributed
                options.UseHybrid(config =>
                {
                    config.TopicName = "test-topic";
                    config.EnableLogging = true;

                    // specify the local cache provider name after v0.5.4
                    config.LocalCacheProviderName = "m1";
                    // specify the distributed cache provider name after v0.5.4

                    config.DistributedCacheProviderName = "myredis";
                }, "h1")
                // use redis bus
                .WithRedisBus(busConf =>
                {
                    busConf.Endpoints.Add(new ServerEndPoint(connectionString, port));
                    busConf.SerializerName = "myjson";
                    busConf.Password = password;
                });
            });
            return services;
        }
    }
}
