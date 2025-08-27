using Microsoft.Data.SqlClient;

namespace IMS.ItemInventory.Api.Shared.Persistence;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}