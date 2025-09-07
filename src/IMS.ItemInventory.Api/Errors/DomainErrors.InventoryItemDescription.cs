namespace IMS.ItemInventory.Api.Errors;

internal static partial class DomainErrors
{
    internal static class InventoryItemDescription
    {
        public static readonly Error Empty = new(
            "InventoryItem.Description.Empty",
            "InventoryItem Description cannot be empty");
    }


    internal static class InventoryItemIdentifier
    {
        public static readonly Error Empty = new(
            "InventoryItem.Identifier.Empty",
            "InventoryItem Identifier cannot be empty");


        public static readonly Func<int, int, Error> InvalidLength = (min, max) => new(
            "InventoryItem.Identifier.InvalidLength",
            $"InventoryItem Identifier length must be between {min} and {max}");
    }
}
