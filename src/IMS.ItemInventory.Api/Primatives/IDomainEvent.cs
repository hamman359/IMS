namespace IMS.ItemInventory.Api.Primatives;

public interface IDomainEvent : INotification //Figure out how to replace MediatR INotification
{
    public Guid Id { get; init; }
}