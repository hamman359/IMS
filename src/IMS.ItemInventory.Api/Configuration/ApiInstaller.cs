using Carter;

using IMS.SharedKernal;
using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

public class ApiInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();

        services.AddCarter(new DependencyContextAssemblyCatalog(
            ItemInventoryAssemblyReference.Assembly,
            SharedKernalAssemblyReference.Assembly));
    }
}