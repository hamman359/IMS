using IMS.ItemInventory.Api.Shared.Results;

namespace IMS.ItemInventory.Api.Shared.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancelationToken);
}
