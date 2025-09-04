using IMS.ItemInventory.Api.Shared.Messaging;

namespace IMS.SharedKernal.Messaging;

public interface IDispatcher
{
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);

    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);

    Task<TResult> SendDomainEventAsync<TResult>(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
