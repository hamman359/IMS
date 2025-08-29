using Microsoft.EntityFrameworkCore;

namespace IMS.ItemInventory.Api.Data;

public class InventoryManagementDbContext : DbContext
{
    public InventoryManagementDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(ItemInventoryAssemblyReference.Assembly);
}