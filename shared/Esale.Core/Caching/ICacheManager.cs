using Esale.Core.Caching;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esale.Core.Caching
{
    public interface ICacheManager
    {
        Task<T?> GetAsync<T>(string key,string prefix, CacheOptions options);
        Task<string?> GetStringAsync(string key, string prefix,CacheOptions options);
        Task SetAsync<T>(string key, string prefix,T value,double ttl, CacheOptions options);
        Task SetStringAsync(string key, string prefix,string value,CacheOptions options,double ttl = 0);
        Task SetWithPrefixKeyAsync(string key, string prefix, string value, double ttl);
        Task RemoveAsync(string key, string prefix,CacheOptions options);
        Task<long> StringIncrementAsync(string key);
        Task<bool> RemoveWithPrefixKeyAsync(string prefixKey);
        Task<bool> RemoveByPrefixAsync(string prefixKey, CacheOptions options);

    }
}
