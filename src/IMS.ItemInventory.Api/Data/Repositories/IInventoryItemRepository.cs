using IMS.ItemInventory.Api.Model.Entities;

namespace IMS.ItemInventory.Api.Data.Repositories;

internal interface IInventoryItemRepository
{
    void Add(InventoryItem item);
}
