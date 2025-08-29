using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Persistence;
using IMS.ItemInventory.Api.Shared.Configuration;
using IMS.ItemInventory.Api.Shared.Persistence;

using Microsoft.EntityFrameworkCore;

using Scrutor;

namespace IMS.ItemInventory.Api.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Scan(
                selector => selector
                    .FromAssemblies(ItemInventoryAssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

        //services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        //services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

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