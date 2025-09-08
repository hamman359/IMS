using Microsoft.EntityFrameworkCore;

namespace IMS.ItemInventory.Api.Data;

internal class InventoryManagementDbContext : DbContext
{
    public InventoryManagementDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder?.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    //Update-Database -a HappyPlate.Persistence -s HappyPlate.App
    //Add-Migration InitialCreate -a HappyPlate.Persistence -s HappyPlate.App
}