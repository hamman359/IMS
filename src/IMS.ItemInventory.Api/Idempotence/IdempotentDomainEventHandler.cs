using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.SharedKernal.Idempotence;

namespace IMS.ItemInventory.Api.Idempotence;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>
    : IdempotentDomainEventHandlerBase<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public IdempotentDomainEventHandler(
    INotificationHandler<TDomainEvent> decorated,
    InventoryManagementDbContext dbContext)
    : base(decorated, dbContext) { }
}
