namespace IMS.ItemInventory.Api.Model.ValueObjects;

internal sealed class InventoryItemName : ValueObject
{
    InventoryItemName(string name)
    {
        Value = name;
    }

    public string Value { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<InventoryItemName> Create(string name)
    {
        return Result.Ensure(
            name,
            (e => name.IsNotNullOrWhiteSpace(), DomainErrors.InventoryItemName.Empty))
            .Map(e => new InventoryItemName(name));
    }
}

