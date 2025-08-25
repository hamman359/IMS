using IMS.ItemInventory.Api.Shared.Messaging;

namespace IMS.ItemInventory.Api.DomainEvents;

public abstract record DomainEvent(Guid Id) : IDomainEvent;