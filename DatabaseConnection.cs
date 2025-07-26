// DatabaseConnection.cs
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IMS_POS
{
    internal class DatabaseConnection
    {
        private readonly string connectionString =
            "Data Source=localhost;Initial Catalog=FinGradProj_DB;Integrated Security=True";

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }

        public async Task<SqlConnection> OpenSqlConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
