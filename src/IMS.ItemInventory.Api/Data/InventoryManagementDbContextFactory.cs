using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IMS.ItemInventory.Api.Data;

//https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
internal class InventoryManagementDbContextFactory
    : IDesignTimeDbContextFactory<InventoryManagementDbContext>
{
    public InventoryManagementDbContext CreateDbContext(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("InventoryDatabase");

        var optionsBuilder = new DbContextOptionsBuilder<InventoryManagementDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new InventoryManagementDbContext(optionsBuilder.Options);
    }
}
