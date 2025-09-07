using IMS.ItemInventory.Api.Persistence.Constants;
using IMS.SharedKernal.Outbox;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IMS.ItemInventory.Api.Persistence.Configuration;
internal sealed class OutboxMessageConsumerConfiguration
    : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable(TableNames.OutboxMessageConsumers);

        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}