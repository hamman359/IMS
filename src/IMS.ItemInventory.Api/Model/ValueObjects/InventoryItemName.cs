namespace IMS.ItemInventory.Api.Model.ValueObjects;

internal sealed class InventoryItemName : ValueObject
{
    InventoryItemName(string name)
    {
        Name = name;
    }

    public string Name { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Name;
    }

    public static Result<InventoryItemName> Create(string name)
    {
        return Result.Ensure(
            name,
            (e => name.IsNotNullOrWhiteSpace(), DomainErrors.InventoryItemName.Empty))
            .Map(e => new InventoryItemName(name));
    }
}

