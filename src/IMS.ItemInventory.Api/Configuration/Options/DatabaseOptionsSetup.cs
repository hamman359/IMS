using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IMS.ItemInventory.Api.Configuration.Options;


internal class DatabaseOptionsSetup(IConfiguration configuration)
    : IConfigureOptions<DatabaseOptions>
{
    const string ConfigurationSectionName = "DatabaseOptions";

    public void Configure(DatabaseOptions options)
    {
        var connectionString = configuration.GetConnectionString("InventoryDatabase");

        options.ConnectionString = connectionString ?? string.Empty;

        configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
