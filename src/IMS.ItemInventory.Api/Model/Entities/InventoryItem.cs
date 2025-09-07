using IMS.ItemInventory.Api.Model.ValueObjects;

namespace IMS.ItemInventory.Api.Model.Entities;

internal class InventoryItem : AggregateRoot
{
    InventoryItem(
        Guid id,
        InventoryItemIdentifier itemIdentifier,
        InventoryItemName name,
        InventoryItemDescription description)
        : base(id)
    {
        ItemIdentifier = itemIdentifier;
        Name = name;
        Description = description;
    }

    public InventoryItemIdentifier ItemIdentifier { get; set; }
    public InventoryItemName Name { get; set; }
    public InventoryItemDescription Description { get; set; }

    public static Result<InventoryItem> Create(
        string sku,
        string name,
        string description)
    {
        Guid id = Guid.CreateVersion7(); //Wrap in service

        Result<InventoryItemIdentifier> skuResult = InventoryItemIdentifier.Create(sku);
        Result<InventoryItemName> nameResult = InventoryItemName.Create(name);
        Result<InventoryItemDescription> descriptionResult = InventoryItemDescription.Create(description);

        var item = new InventoryItem(
            id,
            skuResult.Value,
            nameResult.Value,
            descriptionResult.Value);

        //Raise Events here

        return item;
    }
}

