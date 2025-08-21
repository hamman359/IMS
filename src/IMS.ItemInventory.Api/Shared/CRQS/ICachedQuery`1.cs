namespace IMS.ItemInventory.Api.Shared.CRQS;

public interface ICachedQuery<TResponse> : IQuery<TResponse>
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
