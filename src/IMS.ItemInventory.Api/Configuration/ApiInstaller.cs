using Carter;

using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class ApiInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddEndpointsApiExplorer();

        services.AddCarter(new DependencyContextAssemblyCatalog(
            Api.AssemblyReference.Assembly,
            SharedKernal.AssemblyReference.Assembly));
    }
}