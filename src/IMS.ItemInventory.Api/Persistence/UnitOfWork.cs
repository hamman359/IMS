using IMS.ItemInventory.Api.Data;
using IMS.SharedKernal.Outbox;
using IMS.SharedKernal.Persistence;
using IMS.SharedKernal.Primatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

namespace IMS.ItemInventory.Api.Persistence;

// Wrapper around EFs DbContext.SaveChangesAsync()
// Using this wrapper allows for easier mocking during testing
// and provides an extension point for adding functionality that
// we want to have always occur when changes are persisted to the DB
internal sealed class UnitOfWork(InventoryManagementDbContext dbContext)
    : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        UpdateAuditableEntities();

        return dbContext.SaveChangesAsync(cancellationToken);
    }

    // Loads all of the Domain Events that have been raised by any Aggregate root objects
    // and converts the Domain Events into Outbox Messages before they are persisted t
    // the DB.
    void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                // Since we've already retrieved all of the Domain Evnets from this
                // Aggregate Root, we now want to clear them out so that any additional
                // operations that work with this Aggregate Root will not accidentally
                // result in a Domain Event being processed multiple times.
                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.CreateVersion7(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    // Locates all Entities that have been marked as Auditable and populates
    // Auditing specific fields.
    void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}
