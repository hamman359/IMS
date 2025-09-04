using IMS.SharedKernal.Results;

namespace IMS.SharedKernal.Messaging;

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancelationToken);
}
