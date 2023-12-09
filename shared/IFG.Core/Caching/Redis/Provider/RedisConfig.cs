using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFG.Core.Caching.Redis.Provider
{
    public class RedisConfig
    {
        public string Url { get; set; }
        public int DataBaseId { get; set; } = 0;
        public int Port { get; set; } = 6379;
        public string Password { get; set; }
        public int HybridDataBaseId { get; set; } = 5;
    }
}
