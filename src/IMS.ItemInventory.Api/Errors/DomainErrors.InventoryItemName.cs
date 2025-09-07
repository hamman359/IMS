namespace IMS.ItemInventory.Api.Errors;

internal static partial class DomainErrors
{
    internal static class InventoryItemName
    {
        public static readonly Error Empty = new(
            "InventoryItem.Name.Empty",
            "InventoryItem Name cannot be empty");
    }
}
