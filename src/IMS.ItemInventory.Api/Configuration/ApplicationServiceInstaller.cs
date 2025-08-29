using FluentValidation;


using IMS.ItemInventory.Api.Shared.Configuration;


namespace IMS.ItemInventory.Api.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(
            ItemInventoryAssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}