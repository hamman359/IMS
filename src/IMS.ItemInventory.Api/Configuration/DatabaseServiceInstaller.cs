using IMS.ItemInventory.Api.Configuration.Options;
using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Persistence;
using IMS.SharedKernal.Configuration;
using IMS.SharedKernal.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class DatabaseServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContext<InventoryManagementDbContext>(
            (sp, optionsBuilder) =>
            {
                var databaseOptions = sp.GetService<IOptions<DatabaseOptions>>()!.Value;

                optionsBuilder
                    .UseSqlServer(databaseOptions.ConnectionString, sqlServerAction =>
                    {
                        sqlServerAction
                            .EnableRetryOnFailure(databaseOptions.MaxRetryCount)
                            .CommandTimeout(databaseOptions.CommandTimeout);
                    })
                    .EnableDetailedErrors(databaseOptions.EnableDetailedErrors)
                    .EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
            });

        services.AddScoped<InventoryManagementDbContext>(sp =>
            sp.GetRequiredService<InventoryManagementDbContext>());

        services.AddScoped<IUnitOfWork>(sp =>
            sp.GetRequiredService<UnitOfWork>());

        //services.AddScoped<IFooRepository, FooRepository>();
    }
}