using MediatR;

namespace IMS.ItemInventory.Api.Shared.Messaging;

// Wrapper interface around MediatR's INotification. Used to identify
// Domain Events and to add an Id specific to the Domain Event itself
public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}

