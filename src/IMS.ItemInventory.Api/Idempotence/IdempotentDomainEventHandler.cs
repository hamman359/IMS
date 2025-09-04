using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.SharedKernal.Messaging;
using IMS.SharedKernal.Outbox;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace IMS.ItemInventory.Api.Idempotence;

public sealed class IdempotentDomainEventHandler<TDomainEvent>(
    INotificationHandler<TDomainEvent> decorated,
    InventoryManagementDbContext dbContext)
    : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public async Task Handle(
        TDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var consumer = decorated.GetType().Name;

        if (await dbContext.Set<OutboxMessageConsumer>()
            .AnyAsync(
                o =>
                    o.Id == notification.Id &&
                    o.Name == consumer,
                cancellationToken))
        {
            return;
        }

        await decorated.Handle(notification, cancellationToken);

        dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });
    }
}
