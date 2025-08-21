using System.Reflection;

using FluentValidation;

namespace IMS.ItemInventory.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        var serviceInstallers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(IsAssignableToType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }

        static bool IsAssignableToType<T>(TypeInfo typeinfo) =>
            typeof(T).IsAssignableFrom(typeinfo) &&
            !typeinfo.IsInterface &&
            !typeinfo.IsAbstract;

        return services;
    }
}