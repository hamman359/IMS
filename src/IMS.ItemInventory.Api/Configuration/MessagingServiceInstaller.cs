using FluentValidation;

using IMS.SharedKernal.Behaviors;
using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class MessagingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(AssemblyReference.Assembly);

            //Register all Pipeline behaviors. Ordering of the registration for these matters.

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(QueryCachingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        // Wraps instances of the INotificationHandler with the IdempotentDomainEventHandler
        //services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddValidatorsFromAssembly(
            AssemblyReference.Assembly,
            includeInternalTypes: true);

    }
}
