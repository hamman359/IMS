namespace IMS.ItemInventory.Api.Shared.Configuration;

public interface IServiceInstaller
{
    void Install(IServiceCollection services, IConfiguration configuration);
}