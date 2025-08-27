using Carter;

using IMS.ItemInventory.Api.Shared.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

public class ApiInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddCarter();
    }
}