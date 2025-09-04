using IMS.ItemInventory.Api.Shared.Behaviors;
using IMS.SharedKernal.Caching;
using IMS.SharedKernal.Messaging;
using IMS.SharedKernal.Results;

using MediatR;

using Microsoft.Extensions.Logging;

namespace IMS.SharedKernal.Behaviors;

/// <summary>
/// Defines a MediatR pipeline behavior for handling the Caching of Queries
/// Has Type Constraints to ensure TRequest is an ICachedQuery and that TResponse is a Result.
/// </summary>
public sealed class QueryCachingPipelineBehavior<TRequest, TResponse>(
    ICacheService cacheService,
    ILogger<QueryCachingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TRequest>
    where TResponse : Result
{
    /// <summary>
    /// Checks if the requested data is already cached.
    /// If so, then returns the cached version.
    /// Otherwise, adds the data to the cache.
    /// </summary>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cachedResult = await cacheService.GetAsync<TResponse>(
            request.CacheKey,
            cancellationToken);

        var requestName = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            LogMessage.LogCacheHitForRequest(logger, requestName);

            return cachedResult;
        }

        LogMessage.LogCacheMissForRequest(logger, requestName);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            await cacheService.SetAsync(
                request.CacheKey,
                result,
                request.Expiration,
                cancellationToken);
        }

        return result;
    }
}
