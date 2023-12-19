using IFG.Core.IOC;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IFG.Core.Caching.Redis.Provider
{
    public class MultiplexerProvider
    {
        private readonly IConfiguration _config;
        public string redisConfig { get; set; }
        public MultiplexerProvider(string redisConfig)
        {
            _config = ServiceTool.Resolve<IConfiguration>();
            this.redisConfig = redisConfig;
            var redisCacheSection = _config.GetSection(this.redisConfig);
            var config = redisCacheSection.Get<RedisConfig>();
            var connectionString = "";
            if (config.Password.IsNullOrEmpty())
                connectionString = $"{config.Url}:{config.Port}";
            else
                connectionString = $"{config.Url}:{config.Port},password={config.Password}";
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
        }

        private Lazy<ConnectionMultiplexer> lazyConnection;
        public ConnectionMultiplexer Connection { get { return lazyConnection.Value; } }

        public IDatabase GetDataBase()
        {
            return Connection.GetDatabase();
        }

        public IServer GetServer()
        {
            return Connection.GetServer(_config.GetSection(redisConfig).Value.ToString());
        }
    }
}
