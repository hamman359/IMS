using IMS.ItemInventory.Api.Shared.Messaging;

namespace IMS.SharedKernal.Messaging;

using MediatR;

public interface IDomainEventHandler<in TEvent>
    : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}