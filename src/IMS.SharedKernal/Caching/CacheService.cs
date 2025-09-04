
using System.Collections.Concurrent;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace IMS.SharedKernal.Caching;

// Service for storing and retrieving objects from the Cache
public class CacheService(IDistributedCache distributedCache)
    : ICacheService
{
    // A default caching duration to use if one is not provided by the calling code
    private readonly static TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private readonly static ConcurrentDictionary<string, bool> s_cacheKeys = new();

    // Retrieves a value from the Cache based on the cache key.
    // Returns null if the no value is present in the cache for the provided key.
    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null)
        {
            return null;
        }

        var value = JsonConvert.DeserializeObject<T>(cachedValue);

        return value;
    }

    // Removes the value associated with the provided key from the Cache.
    // This is a "safe" operation in that if no value is present for the provided key
    // Then the call does NOT error and instead simply does nothing.
    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        s_cacheKeys.TryRemove(key, out var _);
    }

    // Removes all items from cache where the cache key starts with the provided prefix.
    // e.g. if the provided prefix is 'foo', then values with keys 'foo', 'foobar', and 'foo-bar'
    // will all be removed from the cache.
    public async Task RemoveByPrefixAsync(
        string prefixKey,
        CancellationToken cancellationToken = default)
    {
        var tasks = s_cacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }

    // Adds the provided value to the cache.
    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration,
        CancellationToken cancellationToken = default) where T : class
    {
        var cacheValue = JsonConvert.SerializeObject(value);

        await distributedCache.SetStringAsync(
            key,
            cacheValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
            },
            cancellationToken);

        s_cacheKeys.TryAdd(key, true);
    }
}
