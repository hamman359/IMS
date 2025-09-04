using IMS.ItemInventory.Api.Idempotence;
using IMS.SharedKernal.Behaviors;
using IMS.SharedKernal.Configuration;

using MediatR;

namespace IMS.ItemInventory.Api.Configuration;

public class MessagingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(ItemInventoryAssemblyReference.Assembly);
            //(typeof(Program).Assembly);

            // Register all Pipeline behaviors. Ordering of the registration for these matters.
            //config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            //config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            //config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));


        // Wraps instances of the INotificationHandler with the IdempotentDomainEventHandler
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
    }
}
