namespace IMS.ItemInventory.Api.Shared.Messaging;

using MediatR;

public interface IDomainEventHandler<in TEvent>
    : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}