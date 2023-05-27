using Esale.Core.Caching.Redis.Provider;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esale.Core.CrossCuttingConcerns.Caching.Redis
{
    public class RedisCacheManager
    {
        private MultiplexerProvider _cacheClient;
        public RedisCacheManager(string redisConfig)
        {
            _cacheClient = new MultiplexerProvider(redisConfig);
        }
        public async Task<bool> StringSetAsync(string key, string value, int duration = 0)
        {
            if (duration > 0)
            {
                return await _cacheClient.GetDataBase().StringSetAsync(key, value.ToString(), TimeSpan.FromSeconds(duration));

            }
            else
            {
                return await _cacheClient.GetDataBase().StringSetAsync(key, value.ToString());

            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cacheClient.GetDataBase().StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _cacheClient.GetDataBase().StringGetAsync(key);
        }

        public async Task<bool> IsAddAsync(string key)
        {
            return await _cacheClient.GetDataBase().KeyExistsAsync(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _cacheClient.GetDataBase().KeyDeleteAsync(key);
        }

        public IEnumerable<RedisKey> SearchKeys(string pattern)
        {
            return _cacheClient.GetServer().Keys(pattern: pattern);
        }

        public string CreateCachKey(string prefix, int userId)
        {
            return prefix + userId.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss", new CultureInfo("fa-IR")) + "_" + Guid.NewGuid().ToString().Substring(0, 8);
        }

        public async Task<long> StringIncrementAsync(string key)
        {
            return await _cacheClient.GetDataBase().StringIncrementAsync(key);
        }
        public long StringIncrement(string key)
        {
            return _cacheClient.GetDataBase().StringIncrement(key);
        }
    }
}
