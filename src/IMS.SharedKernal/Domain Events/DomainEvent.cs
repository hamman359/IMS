
using IMS.ItemInventory.Api.Shared.Messaging;

namespace IMS.ItemInventory.Api.DomainEvents;

public abstract record DomainEvent
    : IDomainEvent
{
    public Guid Id { get; init; }

    protected DomainEvent()
    {
        Id = Guid.CreateVersion7();
    }
}
