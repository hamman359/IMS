namespace IMS.ItemInventory.Api.Model.ValueObjects;

internal sealed class InventoryItemDescription : ValueObject
{
    InventoryItemDescription(string description)
    {
        Value = description;
    }

    public string Value { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<InventoryItemDescription> Create(string description)
    {
        return Result.Ensure(
            description,
            (e => description.IsNotNullOrWhiteSpace(), DomainErrors.InventoryItemDescription.Empty))
            .Map(e => new InventoryItemDescription(description));
    }
}

