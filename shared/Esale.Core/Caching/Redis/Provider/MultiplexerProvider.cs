using Esale.Core.IOC;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esale.Core.Caching.Redis.Provider
{
    public class MultiplexerProvider
    {
        private readonly IConfiguration _config;
        public string redisConfig { get; set; }
        public MultiplexerProvider(string redisConfig)
        {
            _config = ServiceTool.Resolve<IConfiguration>();
            this.redisConfig = redisConfig;
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_config.GetSection(redisConfig).Value.ToString()));
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
