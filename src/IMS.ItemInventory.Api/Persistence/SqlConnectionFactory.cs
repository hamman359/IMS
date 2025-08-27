using IMS.ItemInventory.Api.Shared.Persistence;

using Microsoft.Data.SqlClient;

namespace IMS.ItemInventory.Api.Persistence;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration) =>
        _configuration = configuration;

    public SqlConnection CreateConnection() =>
        new SqlConnection(_configuration.GetConnectionString("Database"));
}
