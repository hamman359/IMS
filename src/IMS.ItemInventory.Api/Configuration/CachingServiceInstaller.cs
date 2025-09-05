using IMS.SharedKernal.Caching;
using IMS.SharedKernal.Configuration;

namespace IMS.ItemInventory.Api.Configuration;

internal sealed class CachingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
    }
}
