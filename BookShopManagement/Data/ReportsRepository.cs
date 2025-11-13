using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class ReportsRepository
    {
        public SalesReport GetSalesReport(DateTime startDate, DateTime endDate)
        {
            var report = new SalesReport
            {
                StartDate = startDate,
                EndDate = endDate
            };

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                // Get summary statistics
                string summaryQuery = @"SELECT 
                                        COUNT(*) AS TotalSales,
                                        ISNULL(SUM(FinalAmount), 0) AS TotalRevenue,
                                        ISNULL(SUM(si.Quantity), 0) AS TotalBooksSold
                                        FROM Sales s
                                        LEFT JOIN SaleItems si ON s.SaleID = si.SaleID
                                        WHERE s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";

                using (var cmd = new SqlCommand(summaryQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            report.TotalSales = reader.GetInt32(0);
                            report.TotalRevenue = reader.GetDecimal(1);
                            report.TotalBooksSold = reader.GetInt32(2);
                        }
                    }
                }

                // Get individual sales
                string salesQuery = @"SELECT * FROM Sales 
                                     WHERE SaleDate >= @StartDate AND SaleDate <= @EndDate
                                     ORDER BY SaleDate DESC";

                using (var cmd = new SqlCommand(salesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            report.Sales.Add(new Sale
                            {
                                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                                CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                                SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                                FinalAmount = reader.GetDecimal(reader.GetOrdinal("FinalAmount")),
                                PaymentMethod = reader.IsDBNull(reader.GetOrdinal("PaymentMethod")) ? null : reader.GetString(reader.GetOrdinal("PaymentMethod"))
                            });
                        }
                    }
                }
            }

            return report;
        }

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

        public Dictionary<string, int> GetTopSellingBooks(int topN = 10)
        {
            var topBooks = new Dictionary<string, int>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT TOP (@TopN) b.Title, SUM(si.Quantity) AS TotalSold
                                FROM SaleItems si
                                JOIN Books b ON si.BookID = b.BookID
                                GROUP BY b.Title
                                ORDER BY TotalSold DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TopN", topN);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topBooks.Add(reader.GetString(0), reader.GetInt32(1));
                        }
                    }
                }
            }
            return topBooks;
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