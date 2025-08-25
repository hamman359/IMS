namespace IMS.ItemInventory.Api.Shared.Messaging;

public interface IDomainEvent
{
    public Guid Id { get; init; }
}

