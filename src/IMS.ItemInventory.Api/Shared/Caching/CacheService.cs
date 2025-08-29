
using System.Collections.Concurrent;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace IMS.ItemInventory.Api.Shared.Caching;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private readonly static ConcurrentDictionary<string, bool> s_cacheKeys = new();

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

    public async Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        s_cacheKeys.TryRemove(key, out var _);
    }

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
