using Microsoft.Data.SqlClient;

namespace Unitess.Repositories
{
    public static class DbConnection
    {
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection("Server=localhost;Database=Unitess;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
