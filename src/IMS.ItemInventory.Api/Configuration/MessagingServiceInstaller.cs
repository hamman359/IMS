using FluentValidation;

using IMS.ItemInventory.Api.Shared.Behaviors;
using IMS.ItemInventory.Api.Shared.Configuration;
using IMS.ItemInventory.Api.Shared.Idempotence;

using MediatR;

namespace IMS.ItemInventory.Api.Configuration;

public class MessagingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(Program).Assembly);

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddValidatorsFromAssembly(
            ItemInventoryAssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}
