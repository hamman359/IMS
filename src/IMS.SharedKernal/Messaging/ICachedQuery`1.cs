namespace IMS.SharedKernal.Messaging;

public interface ICachedQuery<TResponse> : IQuery<TResponse>
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
