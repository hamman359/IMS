using IMS.ItemInventory.Api.Model.Entities;

namespace IMS.ItemInventory.Api.Data.Repositories;

internal sealed class InventoryItemRepository(
    InventoryManagementDbContext dbContext)
    : IInventoryItemRepository
{
    public void Add(InventoryItem item) =>
        dbContext
            .Set<InventoryItem>()
            .Add(item);
}
