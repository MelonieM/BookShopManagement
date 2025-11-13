using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class SalesRepository
    {
        // CREATE SALE
        public int CreateSale(Sale sale)
        {
            var bookRepo = new BookRepository(); // Create here instead

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert Sale header
                        string saleQuery = @"INSERT INTO Sales (CustomerID, SaleDate, TotalAmount, DiscountPercent, FinalAmount, PaymentMethod)
                                            OUTPUT INSERTED.SaleID
                                            VALUES (@CustomerID, @SaleDate, @TotalAmount, @DiscountPercent, @FinalAmount, @PaymentMethod)";

                        int saleID;
                        using (var cmd = new SqlCommand(saleQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CustomerID", sale.CustomerID ?? (object)DBNull.Value);
                            cmd.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
                            cmd.Parameters.AddWithValue("@TotalAmount", sale.TotalAmount);
                            cmd.Parameters.AddWithValue("@DiscountPercent", sale.DiscountPercent);
                            cmd.Parameters.AddWithValue("@FinalAmount", sale.FinalAmount);
                            cmd.Parameters.AddWithValue("@PaymentMethod", sale.PaymentMethod ?? (object)DBNull.Value);

                            saleID = (int)cmd.ExecuteScalar();
                        }

                        // Insert Sale Items and Update Stock
                        foreach (var item in sale.Items)
                        {
                            // Check stock availability
                            var book = bookRepo.GetBookByID(item.BookID);
                            if (book == null || book.StockQuantity < item.Quantity)
                            {
                                throw new Exception($"Insufficient stock for '{item.Book.Title}'. Available: {book?.StockQuantity ?? 0}, Requested: {item.Quantity}");
                            }

                            // Insert Sale Item
                            string itemQuery = @"INSERT INTO SaleItems (SaleID, BookID, Quantity, UnitPrice, Subtotal)
                                                VALUES (@SaleID, @BookID, @Quantity, @UnitPrice, @Subtotal)";
                            using (var cmd = new SqlCommand(itemQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@SaleID", saleID);
                                cmd.Parameters.AddWithValue("@BookID", item.BookID);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                                cmd.Parameters.AddWithValue("@Subtotal", item.Subtotal);
                                cmd.ExecuteNonQuery();
                            }

                            // Update Stock
                            string stockQuery = "UPDATE Books SET StockQuantity = StockQuantity - @Quantity WHERE BookID = @BookID";
                            using (var cmd = new SqlCommand(stockQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@BookID", item.BookID);
                                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return saleID;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Sale GetSaleByID(int saleID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string saleQuery = "SELECT * FROM Sales WHERE SaleID = @SaleID";
                Sale sale = null;

                using (var cmd = new SqlCommand(saleQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SaleID", saleID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sale = MapSale(reader);
                        }
                    }
                }

                if (sale == null) return null;

                string itemsQuery = @"SELECT si.*, b.Title, b.Author, b.ISBN 
                                     FROM SaleItems si
                                     JOIN Books b ON si.BookID = b.BookID
                                     WHERE si.SaleID = @SaleID";

                using (var cmd = new SqlCommand(itemsQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SaleID", saleID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sale.Items.Add(MapSaleItem(reader));
                        }
                    }
                }

                return sale;
            }
        }

        public List<Sale> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            var sales = new List<Sale>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT * FROM Sales 
                                WHERE SaleDate >= @StartDate AND SaleDate <= @EndDate
                                ORDER BY SaleDate DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(MapSale(reader));
                        }
                    }
                }
            }
            return sales;
        }

        public List<Sale> GetCustomerPurchaseHistory(int customerID)
        {
            var sales = new List<Sale>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Sales WHERE CustomerID = @CustomerID ORDER BY SaleDate DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sales.Add(MapSale(reader));
                        }
                    }
                }
            }
            return sales;
        }

        private Sale MapSale(SqlDataReader reader)
        {
            return new Sale
            {
                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                CustomerID = reader.IsDBNull(reader.GetOrdinal("CustomerID")) ? null : reader.GetInt32(reader.GetOrdinal("CustomerID")),
                SaleDate = reader.GetDateTime(reader.GetOrdinal("SaleDate")),
                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                DiscountPercent = reader.GetDecimal(reader.GetOrdinal("DiscountPercent")),
                FinalAmount = reader.GetDecimal(reader.GetOrdinal("FinalAmount")),
                PaymentMethod = reader.IsDBNull(reader.GetOrdinal("PaymentMethod")) ? null : reader.GetString(reader.GetOrdinal("PaymentMethod"))
            };
        }

        private SaleItem MapSaleItem(SqlDataReader reader)
        {
            return new SaleItem
            {
                SaleItemID = reader.GetInt32(reader.GetOrdinal("SaleItemID")),
                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                Book = new Book
                {
                    BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                    Title = reader.GetString(reader.GetOrdinal("Title")),
                    Author = reader.GetString(reader.GetOrdinal("Author")),
                    ISBN = reader.GetString(reader.GetOrdinal("ISBN"))
                }
            };
        }
    }
}