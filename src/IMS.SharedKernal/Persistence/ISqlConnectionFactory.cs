using Microsoft.Data.SqlClient;

namespace IMS.SharedKernal.Persistence;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}