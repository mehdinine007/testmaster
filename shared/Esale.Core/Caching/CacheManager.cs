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

namespace Esale.Core.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IHybridCachingProvider _hybridCache;
        public CacheManager(IDistributedCache distributedCache, IHybridCachingProvider hybridCache)
        {
            _distributedCache = distributedCache;
            _hybridCache = hybridCache;
        }
        public async Task<T?> GetAsync<T>(string key, string prefix, CacheProviderEnum provider = CacheProviderEnum.Redis)
        {
            var getValue = await _hybridCache.GetAsync<T>(prefix+key);
            if (getValue.HasValue)
                return getValue.Value;
            return default(T);
        }

        public async Task<string?> GetStringAsync(string key, string prefix, CacheProviderEnum provider = CacheProviderEnum.Redis)
        {
            if (provider == CacheProviderEnum.Redis)
                return await _distributedCache.GetStringAsync(prefix + key);
            else if (provider == CacheProviderEnum.Hybrid)
                return await GetAsync<string>(key, prefix);
            return null;
        }

        public async Task RemoveAsync(string key, string prefix, CacheProviderEnum provider = CacheProviderEnum.Redis)
        {
            if (provider == CacheProviderEnum.Redis)
                await _distributedCache.RemoveAsync(prefix + key);
            else if (provider == CacheProviderEnum.Hybrid)
                await _hybridCache.RemoveAsync(prefix+key);
        }

        public async Task SetAsync<T>(string key, string prefix, T value, double ttl = 0, CacheProviderEnum provider = CacheProviderEnum.Redis)
        {
            await _hybridCache.SetAsync(prefix + key, value, TimeSpan.FromSeconds(ttl));
        }
    }
}
