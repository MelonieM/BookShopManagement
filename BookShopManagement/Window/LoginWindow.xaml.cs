using System;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using BookShopManagement.Models;

namespace BookShopManagement
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = "";

            try
            {
                string connStr = "Server=(localdb)\\projectModels;Database=BookShopDB;Integrated Security=true;";
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    var cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn);
                    int count = (int)cmd.ExecuteScalar();

                    MessageBox.Show($"SUCCESS!\n\nDatabase connected.\nFound {count} users.\n\nYou can now login.", "Test Passed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error:\n\n{ex.Message}\n\nMake sure:\n1. SQL Server is running\n2. BookShopDB exists\n3. Users table exists", "Test Failed");
            }
        }

        private void Login()
        {
            TxtError.Text = "";

            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username))
            {
                TxtError.Text = "Enter username";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Enter password";
                return;
            }

            try
            {
                BtnLogin.IsEnabled = false;
                BtnLogin.Content = "Logging in...";

                // Hash password
                string hash = GetMD5Hash(password);

                // Connect to database
                string connStr = "Server=(localdb)\\projectModels;Database=BookShopDB;Integrated Security=true;";
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"SELECT UserID, Username, FullName, Role, IsActive 
                                    FROM Users 
                                    WHERE Username = @Username 
                                    AND PasswordHash = @Hash 
                                    AND IsActive = 1";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Hash", hash);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Login successful
                                var user = new User
                                {
                                    UserID = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    FullName = reader.GetString(2),
                                    Role = reader.GetString(3),
                                    IsActive = reader.GetBoolean(4)
                                };

                                CurrentUser.LoggedInUser = user;

                                MessageBox.Show($"Welcome, {user.FullName}!\n\nRole: {user.Role}", "Login Success");

                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                TxtError.Text = "Invalid username or password!";
                                TxtPassword.Password = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TxtError.Text = "Login failed!";
                MessageBox.Show($"Error: {ex.Message}", "Login Error");
            }
            finally
            {
                BtnLogin.IsEnabled = true;
                BtnLogin.Content = "LOGIN";
            }
        }

        private string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
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