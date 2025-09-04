using System.Reflection;

using FluentValidation;

using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

public static class DependencyInjection
{
    // This extension method allows for the automatic adding of configuration settings
    // from files other than Program.cs. This also allows files with configuration settings
    // to be loaded from other assemblies.
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // Scans the list of provided assemblies for all classes that implement IServiceInstaller
        // and instantiates an instance of each IServiceInstaller implementation.
        var serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        // Loops through all of the Installer classes created above and calls their install
        // method which will registerany services configured in that class.
        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }

        return services;
    }

    // Helper method to filter out any Interfaces or Abstract classes that might
    // implement IServiceInstaller
    private static bool IsAssignableToType<T>(TypeInfo typeinfo)
    {
        return typeof(T).IsAssignableFrom(typeinfo) &&
                    !typeinfo.IsInterface &&
                    !typeinfo.IsAbstract;
    }
}