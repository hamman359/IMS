using IMS.ItemInventory.Api.Model.ValueObjects;

namespace IMS.ItemInventory.Api.Model.Entities;

internal class InventoryItem : AggregateRoot
{
    InventoryItem(
        InventoryItemIdentifier itemIdentifier,
        InventoryItemName name,
        InventoryItemDescription description)
        : base()
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
        List<Error> errors = [];

        Result<InventoryItemIdentifier> skuResult = InventoryItemIdentifier.Create(sku);

        if (skuResult.IsFailure)
        {
            errors.AddRange(skuResult.Errors);
        }

        Result<InventoryItemName> nameResult = InventoryItemName.Create(name);

        if (nameResult.IsFailure)
        {
            errors.AddRange(nameResult.Errors);
        }

        Result<InventoryItemDescription> descriptionResult = InventoryItemDescription.Create(description);

        if (descriptionResult.IsFailure)
        {
            errors.AddRange(descriptionResult.Errors);
        }

        if (errors.Count > 0)
        {
            return Result.Failure<InventoryItem>(errors.ToArray());
        }

        var item = new InventoryItem(
            skuResult.Value,
            nameResult.Value,
            descriptionResult.Value);

        //Raise Events here

        return item;
    }
}

