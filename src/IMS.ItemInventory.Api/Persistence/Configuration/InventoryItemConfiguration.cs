using IMS.ItemInventory.Api.Model.Entities;
using IMS.ItemInventory.Api.Model.ValueObjects;
using IMS.ItemInventory.Api.Persistence.Constants;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IMS.ItemInventory.Api.Persistence.Configuration;

internal sealed class InventoryItemConfiguration
    : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable(TableNames.InventoryItems);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.ItemIdentifier)
            .HasConversion(x => x.Sku, x => InventoryItemIdentifier.Create(x).Value)
            .HasMaxLength(50);

        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, x => InventoryItemName.Create(x).Value)
            .HasMaxLength(100);

        builder
            .Property(x => x.Description)
            .HasConversion(x => x.Value, x => InventoryItemDescription.Create(x).Value);
    }
}
