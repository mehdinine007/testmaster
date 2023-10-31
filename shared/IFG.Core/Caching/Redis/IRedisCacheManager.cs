using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFG.Core.Caching.Redis
{
    public interface IRedisCacheManager
    {
        Task<bool> StringSetAsync(string key, string value, int duration = 0);
        Task StringAppendAsync(string key, string value, int duration = 0);
        Task<T> GetAsync<T>(string key);
        Task<string> GetStringAsync(string key);
        Task<bool> IsAddAsync(string key);
        Task<bool> RemoveAsync(string key);
        IEnumerable<string> SearchKeys(string pattern);
        Task<bool> RemoveAllAsync(string pattern);
        Task<bool> RemoveWithPrefixKeyAsync(string prefixKey);
        string CreateCachKey(string prefix, int userId);
        Task<long> StringIncrementAsync(string key);
        long StringIncrement(string key);
        Task<List<string>> ScanKeysAsync(string match, int count=250);
    }
}
