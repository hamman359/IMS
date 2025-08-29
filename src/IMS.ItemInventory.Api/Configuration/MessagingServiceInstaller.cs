using FluentValidation;

using IMS.ItemInventory.Api.Data;
using IMS.ItemInventory.Api.Shared.Behaviors;
using IMS.ItemInventory.Api.Shared.Configuration;
using IMS.ItemInventory.Api.Shared.Idempotence;

namespace IMS.ItemInventory.Api.Configuration;

public class MessagingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<InventoryManagementDbContext>();

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(IdempotentDomainEventHandler<>));
            config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(
            ItemInventoryAssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}
