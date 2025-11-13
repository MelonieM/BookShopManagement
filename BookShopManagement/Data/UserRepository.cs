using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement.Data
{
    public class UserRepository
    {
        // Authenticate user
        public User? AuthenticateUser(string username, string password)
        {
            string passwordHash = HashPassword(password);

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"SELECT * FROM Users 
                                WHERE Username = @Username 
                                AND PasswordHash = @PasswordHash 
                                AND IsActive = 1";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var user = MapUser(reader);

                            // Update last login
                            UpdateLastLogin(user.UserID);

                            // Log successful login
                            LogLogin(user.UserID, "Success");

                            return user;
                        }
                    }
                }
            }

            // Log failed login attempt
            LogLogin(0, "Failed");
            return null;
        }

        // Get all users
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Users ORDER BY FullName";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(MapUser(reader));
                    }
                }
            }
            return users;
        }

        // Add new user
        public bool AddUser(User user, string password)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Users (Username, PasswordHash, FullName, Role, IsActive)
                                VALUES (@Username, @PasswordHash, @FullName, @Role, @IsActive)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Update user
        public bool UpdateUser(User user)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Users 
                                SET FullName = @FullName, 
                                    Role = @Role, 
                                    IsActive = @IsActive
                                WHERE UserID = @UserID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Change password
        public bool ChangePassword(int userID, string newPassword)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@PasswordHash", HashPassword(newPassword));

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Check if username exists
        public bool UsernameExists(string username, int excludeUserID = 0)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND UserID != @ExcludeID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@ExcludeID", excludeUserID);

                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

        // Helper methods
        private void UpdateLastLogin(int userID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Users SET LastLogin = GETDATE() WHERE UserID = @UserID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LogLogin(int userID, string status)
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO LoginHistory (UserID, LoginStatus)
                                    VALUES (@UserID, @Status)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID == 0 ? (object)DBNull.Value : userID);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { /* Log error if needed */ }
        }

        private User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                LastLogin = reader.IsDBNull(reader.GetOrdinal("LastLogin")) ? null : reader.GetDateTime(reader.GetOrdinal("LastLogin"))
            };
        }

        // Simple MD5 hashing (For demo purposes only! Use bcrypt or Argon2 in production)
        private string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}