using EasyCaching.Core;
using Esale.Core.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polly.Caching;
using Esale.Core.Caching.Redis;

namespace Esale.Core.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHybridCachingProvider _hybridCache;
        private readonly IRedisCacheManager _redisCacheManager;
        public CacheManager(IDistributedCache distributedCache, IHybridCachingProvider hybridCache, IRedisCacheManager redisCacheManager)
        {
            _distributedCache = distributedCache;
            _hybridCache = hybridCache;
            _redisCacheManager = redisCacheManager;
        }
        public async Task<T?> GetAsync<T>(string key, string prefix, CacheOptions options)
        {
            if (options.Provider == CacheProviderEnum.Hybrid)
            {
                var getValue = await _hybridCache.GetAsync<T>(prefix + key);
                if (getValue.HasValue)
                    return getValue.Value;
            }
            return default(T);
        }

        public async Task<string?> GetStringAsync(string key, string prefix, CacheOptions options)
        {
            if (options.Provider == CacheProviderEnum.Redis)
            {
                if (options.RedisHash)
                    return await _distributedCache.GetStringAsync(prefix + key);
                else
                    return await _redisCacheManager.GetStringAsync(prefix + key);
            }
            else if (options.Provider == CacheProviderEnum.Hybrid)
                return await GetAsync<string>(key, prefix, options);
            return null;
        }

        public async Task RemoveAsync(string key, string prefix, CacheOptions options)
        {
            if (options.Provider == CacheProviderEnum.Redis)
                await _distributedCache.RemoveAsync(prefix + key);
            else if (options.Provider == CacheProviderEnum.Hybrid)
                await _hybridCache.RemoveAsync(prefix + key);
        }

        public async Task<bool> RemoveWithPrefixKeyAsync(string prefixKey)
        {
            return await _redisCacheManager.RemoveWithPrefixKeyAsync(prefixKey);
        }

        public async Task SetAsync<T>(string key, string prefix, T value, double ttl, CacheOptions options)
        {
            if (options.Provider == CacheProviderEnum.Hybrid)
                await _hybridCache.SetAsync(prefix + key, value, TimeSpan.FromSeconds(ttl));
        }

        public async Task SetStringAsync(string key, string prefix, string value, CacheOptions options, double ttl = 0)
        {
            if (options.Provider == CacheProviderEnum.Redis)
            {
                if (options.RedisHash)
                {
                    if (ttl != 0)
                        await _distributedCache.SetStringAsync(prefix + key, value, new DistributedCacheEntryOptions
                        {
                            AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(ttl))
                        });
                    else
                        await _distributedCache.SetStringAsync(prefix + key, value);
                }
                else
                    await _redisCacheManager.StringSetAsync(prefix + key, value, (int)ttl);
            }
            else if (options.Provider == CacheProviderEnum.Hybrid)
            {
                await _hybridCache.SetAsync(prefix + key, value, TimeSpan.FromSeconds(ttl));
            }
        }

        public async Task SetWithPrefixKeyAsync(string key, string prefix, string value, double ttl)
        {
            await SetStringAsync(key, prefix, value, new CacheOptions()
            {
                Provider = CacheProviderEnum.Redis
            }, ttl);
            string cacheKeyName = prefix + key;
            await SetStringAsync(prefix, "", cacheKeyName + ",", new CacheOptions()
            {
                Provider = CacheProviderEnum.Redis,
                RedisHash = false
            });
        }

        public async Task<long> StringIncrementAsync(string key)
        {
            return await _redisCacheManager.StringIncrementAsync(key);
        }
    }
}
