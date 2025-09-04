using Microsoft.Extensions.Logging;

namespace IMS.ItemInventory.Api.Shared.Behaviors;

// Class used by Compile-time logging source generation to generate the logging messages.
public static partial class LogMessage
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