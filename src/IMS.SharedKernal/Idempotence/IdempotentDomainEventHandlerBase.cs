using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.SharedKernal.Outbox;

using Microsoft.EntityFrameworkCore;

namespace IMS.SharedKernal.Idempotence;
public class IdempotentDomainEventHandlerBase<TDomainEvent>(
    INotificationHandler<TDomainEvent> decorated,
    DbContext dbContext)
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