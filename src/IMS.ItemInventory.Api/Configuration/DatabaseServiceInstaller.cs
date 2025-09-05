using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Persistence;
using IMS.SharedKernal.Configuration;
using IMS.SharedKernal.Persistence;

using Microsoft.EntityFrameworkCore;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class DatabaseServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryManagementDbContext>(
            (sp, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"), default);
            });

        services.AddScoped<InventoryManagementDbContext>(sp =>
            sp.GetRequiredService<InventoryManagementDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<UnitOfWork>());

        //services.AddScoped<IFooRepository, FooRepository>();
    }
}