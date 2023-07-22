using Esale.Core.Caching.Redis.Provider;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esale.Core.Caching.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private MultiplexerProvider _cacheClient;
        public RedisCacheManager()
        {
            _cacheClient = new MultiplexerProvider("RedisCache:ConnectionString");
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

        public IEnumerable<string> SearchKeys(string pattern)
        {
            var keys= _cacheClient.GetServer().Keys(pattern: pattern);
            var listKeys = new List<string>();
            listKeys.AddRange(keys.Select(key => (string)key).ToList());
            return listKeys;
        }

        public async Task<bool> RemoveAllAsync(string pattern)
        {
            var keys = _cacheClient.GetServer().Keys(pattern: pattern);
            var listKeys = new List<string>();
            listKeys.AddRange(keys.Select(key => (string)key).ToList());
            if (listKeys !=null && listKeys.Count > 0)
            {
                foreach (var key in listKeys)
                {
                    await RemoveAsync(key);
                }
            }
            return true;
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

        public async Task<List<string>> ScanKeysAsync(string match, int count = 250)
        {
            var schemas = new List<string>();
            int nextCursor = 0;
            do
            {
                RedisResult redisResult = await _cacheClient.GetDataBase().ExecuteAsync("SCAN", nextCursor.ToString(), "MATCH", match, "COUNT", count.ToString());
                var innerResult = (RedisResult[])redisResult;

                nextCursor = int.Parse((string)innerResult[0]);

                List<string> resultLines = ((string[])innerResult[1]).ToList();
                schemas.AddRange(resultLines);
            }
            while (nextCursor != 0);

            return schemas;
        }

        public async Task StringAppendAsync(string key, string value, int duration = 0)
        {
            await _cacheClient.GetDataBase().StringAppendAsync(key, value.ToString());
        }

        public async Task<bool> RemoveWithPrefixKeyAsync(string prefix)
        {
            var cacheKeys = new List<string>();
            string getCacheKeys = await GetStringAsync(prefix);
            if (string.IsNullOrEmpty(getCacheKeys))
                return false;
            cacheKeys = getCacheKeys.Substring(0, getCacheKeys.Length - 1).Split(",").ToList();
            foreach (var key in cacheKeys)
            {
                await RemoveAsync(key);
            }
            return await RemoveAsync(prefix);
        }
    }
}
