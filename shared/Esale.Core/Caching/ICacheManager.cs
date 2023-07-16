using Esale.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esale.Core.Caching
{
    public interface ICacheManager
    {
        Task<T?> GetAsync<T>(string key,string prefix, CacheProviderEnum provider = CacheProviderEnum.Redis);
        Task<string?> GetStringAsync(string key, string prefix, CacheProviderEnum provider = CacheProviderEnum.Redis);
        Task SetAsync<T>(string key, string prefix,T value,double ttl = 0,CacheProviderEnum provider = CacheProviderEnum.Redis);
        Task RemoveAsync(string key, string prefix,CacheProviderEnum provider = CacheProviderEnum.Redis);
    }
}
