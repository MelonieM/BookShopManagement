using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class ReportsRepository
    {
        public List<LowStockBook> GetLowStockBooks(int threshold = 10)
        {
            var lowStockBooks = new List<LowStockBook>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT BookID, ISBN, Title, Author, StockQuantity, Price
                                FROM Books
                                WHERE StockQuantity < @Threshold
                                ORDER BY StockQuantity ASC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Threshold", threshold);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lowStockBooks.Add(new LowStockBook
                            {
                                BookID = reader.GetInt32(0),
                                ISBN = reader.GetString(1),
                                Title = reader.GetString(2),
                                Author = reader.GetString(3),
                                StockQuantity = reader.GetInt32(4),
                                Price = reader.GetDecimal(5)
                            });
                        }
                    }
                }
            }
            return lowStockBooks;
        }

        public decimal GetTotalRevenueToday()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT ISNULL(SUM(FinalAmount), 0)
                                FROM Sales
                                WHERE CAST(SaleDate AS DATE) = CAST(GETDATE() AS DATE)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    return (decimal)cmd.ExecuteScalar();
                }
            }
        }
    }
}
