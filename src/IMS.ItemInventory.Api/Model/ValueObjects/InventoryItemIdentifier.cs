namespace IMS.ItemInventory.Api.Model.ValueObjects;

internal sealed class InventoryItemIdentifier : ValueObject
{
    const int MinimumLength = 2;
    const int MaximumLength = 50;

    InventoryItemIdentifier(string sku)
    {
        Sku = sku;
    }

    public string Sku { get; init; }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Sku;
    }

    public static Result<InventoryItemIdentifier> Create(string sku)
    {
        return Result.Ensure(
            sku,
            (e => sku.IsNotNullOrWhiteSpace(), DomainErrors.InventoryItemIdentifier.Empty),
            (e =>
                sku.LengthIsBetweenMinAndMax(MinimumLength, MaximumLength),
                DomainErrors.InventoryItemIdentifier.InvalidLength(MinimumLength, MaximumLength)))
            //(e => SkuIsUnique(sku), new Error("c", "c")))
            .Map(e => new InventoryItemIdentifier(sku));
    }
}

