using FluentValidation;

using IMS.SharedKernal.Configuration;

using Scrutor;


namespace IMS.ItemInventory.Api.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //Scans the provided assemblies and automatically registers everything it finds.
        // This uses the Scrutor library, which adds Assembly Scanning and Decoration
        // extentions to the built in Microsoft Dependency Injection.
        // More information about Scrutor available at https://github.com/khellang/Scrutor
        services
            .Scan(
                selector => selector
                    .FromAssemblies(ItemInventoryAssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime());

        services.AddValidatorsFromAssembly(
            ItemInventoryAssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}