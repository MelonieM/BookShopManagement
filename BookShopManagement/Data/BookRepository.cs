using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            var books = new List<Book>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Books ORDER BY Title";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(MapBook(reader));
                    }
                }
            }
            return books;
        }

        public Book? GetBookByID(int bookID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Books WHERE BookID = @BookID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapBook(reader);
                    }
                }
            }
            return null;
        }

        public bool AddBook(Book book)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Books (ISBN, Title, Author, Price, StockQuantity, Category, Publisher, PublishedYear)
                                VALUES (@ISBN, @Title, @Author, @Price, @Stock, @Category, @Publisher, @Year)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@Stock", book.StockQuantity);
                    cmd.Parameters.AddWithValue("@Category", book.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Publisher", book.Publisher ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Year", book.PublishedYear ?? (object)DBNull.Value);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBook(Book book)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Books SET ISBN=@ISBN, Title=@Title, Author=@Author, 
                                Price=@Price, StockQuantity=@Stock, Category=@Category, 
                                Publisher=@Publisher, PublishedYear=@Year, UpdatedDate=GETDATE()
                                WHERE BookID=@BookID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", book.BookID);
                    cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@Stock", book.StockQuantity);
                    cmd.Parameters.AddWithValue("@Category", book.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Publisher", book.Publisher ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Year", book.PublishedYear ?? (object)DBNull.Value);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBook(int bookID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Books WHERE BookID = @BookID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateStock(int bookID, int quantity)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Books SET StockQuantity = StockQuantity + @Quantity WHERE BookID = @BookID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", bookID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            var books = new List<Book>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT * FROM Books 
                                WHERE Title LIKE @Search OR Author LIKE @Search OR ISBN LIKE @Search
                                ORDER BY Title";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Search", $"%{searchTerm}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            books.Add(MapBook(reader));
                        }
                    }
                }
            }
            return books;
        }

        private Book MapBook(SqlDataReader reader)
        {
            return new Book
            {
                BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                Title = reader.GetString(reader.GetOrdinal("Title")),
                Author = reader.GetString(reader.GetOrdinal("Author")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? null : reader.GetString(reader.GetOrdinal("Category")),
                Publisher = reader.IsDBNull(reader.GetOrdinal("Publisher")) ? null : reader.GetString(reader.GetOrdinal("Publisher")),
                PublishedYear = reader.IsDBNull(reader.GetOrdinal("PublishedYear")) ? null : reader.GetInt32(reader.GetOrdinal("PublishedYear")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate"))
            };
        }
    }
}