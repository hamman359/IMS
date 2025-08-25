namespace IMS.ItemInventory.Api.Shared.Messaging;
using System.Threading;
using System.Threading.Tasks;

public interface IDomainEventHandler<in TDomainEvent>
    where TDomainEvent : IRequest
{

    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}