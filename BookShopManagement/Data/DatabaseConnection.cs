<<<<<<< HEAD
﻿using Microsoft.Data.SqlClient;
=======
﻿using System.Data.SqlClient;
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707

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