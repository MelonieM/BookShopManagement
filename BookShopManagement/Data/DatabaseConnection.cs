using Microsoft.Data.SqlClient;

namespace BookShopManagement.Data
{
    public class DatabaseConnection
    {
        private static string connectionString =
            "Server=(localdb)\\projectModels;Database=BookShopDB;Integrated Security=true;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
