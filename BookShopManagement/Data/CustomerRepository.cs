using System;
using System.Collections.Generic;
<<<<<<< HEAD
using Microsoft.Data.SqlClient;
=======
using System.Data.SqlClient;
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class CustomerRepository
    {
        public List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Customers ORDER BY Name";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(MapCustomer(reader));
                    }
                }
            }
            return customers;
        }

        public Customer GetCustomerByID(int customerID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return MapCustomer(reader);
                    }
                }
            }
            return null;
        }

        public bool AddCustomer(Customer customer)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Customers (Name, Contact, Email, Address)
                                VALUES (@Name, @Contact, @Email, @Address)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Contact", customer.Contact);
                    cmd.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Customers SET Name=@Name, Contact=@Contact, 
                                Email=@Email, Address=@Address WHERE CustomerID=@CustomerID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                    cmd.Parameters.AddWithValue("@Name", customer.Name);
                    cmd.Parameters.AddWithValue("@Contact", customer.Contact);
                    cmd.Parameters.AddWithValue("@Email", customer.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private Customer MapCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Contact = reader.GetString(reader.GetOrdinal("Contact")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
            };
        }
    }
}