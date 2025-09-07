using IMS.SharedKernal.Primatives;

namespace IMS.ItemInventory.Api.Model.ValueObjects;

internal sealed class InventoryItemDescription : ValueObject
{
    InventoryItemDescription(string description)
    {
        Description = description;
    }

    public string Description { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Description;
    }

    public static Result<InventoryItemDescription> Create(string description)
    {
        return Result.Ensure(
            description,
            (e => !string.IsNullOrWhiteSpace(description), new Error("a", "A")))
            .Map(e => new InventoryItemDescription(description));
    }
}

