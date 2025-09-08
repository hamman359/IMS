using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class ApiInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddEndpointsApiExplorer();

        //services.AddOpenApi();

        //services.AddCarter(new DependencyContextAssemblyCatalog(
        //    Api.AssemblyReference.Assembly,
        //    SharedKernal.AssemblyReference.Assembly));
    }
}