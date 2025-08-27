using IMS.ItemInventory.Api.Shared.Configuration;
using IMS.ItemInventory.Api.Shared.Messaging;

namespace IMS.ItemInventory.Api.Configuration;

public class MessagingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(Dispatcher))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
            .AddClasses(classes => classes.AssignableTo(typeof(IDispatcher)), publicOnly: false)
                .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}
