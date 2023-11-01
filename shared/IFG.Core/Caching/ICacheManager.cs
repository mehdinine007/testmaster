
namespace IFG.Core.Caching
{
    public interface ICacheManager
    {
        Task<T?> GetAsync<T>(string key,string prefix, CacheOptions options);
        T? Get<T>(string key, string prefix, CacheOptions options);
        Task<string?> GetStringAsync(string key, string prefix,CacheOptions options);
        string GetString(string key, string prefix, CacheOptions options);
        Task SetAsync<T>(string key, string prefix,T value,double ttl, CacheOptions options);
        Task SetStringAsync(string key, string prefix,string value,CacheOptions options,double ttl = 0);
        Task SetWithPrefixKeyAsync(string key, string prefix, string value, double ttl);
        Task RemoveAsync(string key, string prefix,CacheOptions options);
        Task<long> StringIncrementAsync(string key);
        Task<bool> RemoveWithPrefixKeyAsync(string prefixKey);
        Task<bool> RemoveByPrefixAsync(string prefixKey, CacheOptions options);
        Task<bool> KeyExistsAsync(string key,string prefix, CacheOptions options);
        bool KeyExists(string key,string prefix, CacheOptions options);

    }
}
