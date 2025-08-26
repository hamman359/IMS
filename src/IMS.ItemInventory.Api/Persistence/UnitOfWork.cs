using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Messaging;
using IMS.ItemInventory.Api.Shared.Outbox;
using IMS.ItemInventory.Api.Shared.Persistence;
using IMS.ItemInventory.Api.Shared.Primatives;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Newtonsoft.Json;

namespace IMS.ItemInventory.Api.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    readonly InventoryManagementDbContext _dbContext;

    public UnitOfWork(InventoryManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDomainEventsToOutboxMessages();
        UpdateAuditableEntities();

        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    void ConvertDomainEventsToOutboxMessages()
    {
        var outboxMessages = _dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
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

        _dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
    }

    void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _dbContext
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
