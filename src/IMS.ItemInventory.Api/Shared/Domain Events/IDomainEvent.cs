using MediatR;

namespace IMS.ItemInventory.Api.Shared.Messaging;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}

