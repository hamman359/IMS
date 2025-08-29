using IMS.ItemInventory.Api.Shared.Caching;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.ItemInventory.Api.Shared.Results;

using MediatR;

namespace IMS.ItemInventory.Api.Shared.Behaviors;

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
        TResponse? cachedResult = await cacheService.GetAsync<TResponse>(
            request.CacheKey,
            cancellationToken);

        string requestName = typeof(TRequest).Name;

        if (cachedResult is not null)
        {
            LoggerMessages.LogCacheHitForRequest(logger, requestName);

            return cachedResult;
        }

        LoggerMessages.LogCacheMissForRequest(logger, requestName);

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

public static partial class LoggerMessages
{
    [LoggerMessage(
        EventId = 0,
        EventName = "Cache Hit",
        Level = LogLevel.Information,
        Message = "Cache hit for {RequestName}")]
    public static partial void LogCacheHitForRequest(
        ILogger logger,
        string RequestName);

    [LoggerMessage(
        EventId = 0,
        EventName = "Cache Miss",
        Level = LogLevel.Information,
        Message = "Cache miss for {RequestName}")]
    public static partial void LogCacheMissForRequest(
        ILogger logger,
        string RequestName);
}