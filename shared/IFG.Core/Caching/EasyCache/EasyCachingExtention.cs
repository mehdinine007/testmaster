using EasyCaching.Core.Configurations;
using IFG.Core.Caching.Redis.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
namespace IFG.Core.Caching
{
    public static class EasyCachingExtention
    {
        public static IServiceCollection EasyCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSection = configuration.GetSection("RedisCache");
            var redisConfig = redisCacheSection.Get<RedisConfig>();
           
            services.AddEasyCaching(options =>
            {
                options.WithJson("myjson");

                // local
                options.UseInMemory("m1");
                // distributed
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(redisConfig.Url, redisConfig.Port));

                    config.DBConfig.Database = redisConfig.HybridDataBaseId;

                    config.SerializerName = "myjson";

                    config.DBConfig.Password = redisConfig.Password;
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
                    busConf.Endpoints.Add(new ServerEndPoint(redisConfig.Url, redisConfig.Port));
                    busConf.SerializerName = "myjson";
                    busConf.Password = redisConfig.Password;
               
                });
            });
            return services;
        }
    }
}
