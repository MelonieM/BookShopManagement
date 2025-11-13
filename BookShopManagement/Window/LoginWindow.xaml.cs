using System;
using System.Windows;
using System.Windows.Input;
using BookShopManagement.Data;
using BookShopManagement.Models;

namespace BookShopManagement
{
    public partial class LoginWindow : Window
    {
        private UserRepository userRepo;

        public LoginWindow()
        {
            InitializeComponent();
            userRepo = new UserRepository();
            TxtUsername.Focus();

            // Test database connection on load
            TestDatabaseConnection();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // Connection successful
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"⚠️ Database Connection Error:\n\n{ex.Message}\n\nPlease check:\n1. SQL Server is running\n2. Database 'BookShopDB' exists\n3. Users table exists",
                              "Database Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
        }

        private void LinkForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Password Recovery:\n\n" +
                "Please contact your system administrator to reset your password.\n\n" +
                "Default Passwords:\n" +
                "• admin: admin123\n" +
                "• manager: manager123\n" +
                "• cashier: cashier123",
                "Forgot Password",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void AttemptLogin()
        {
            // Clear previous error
            TxtError.Text = "";

            // Validate input
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(username))
            {
                TxtError.Text = "❌ Please enter username";
                TxtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                TxtError.Text = "❌ Please enter password";
                TxtPassword.Focus();
                return;
            }

            try
            {
                // Disable login button during authentication
                BtnLogin.IsEnabled = false;
                BtnLogin.Content = "⏳ Logging in...";
                BtnLogin.Background = System.Windows.Media.Brushes.Gray;

                // Show what we're trying to authenticate
                System.Diagnostics.Debug.WriteLine($"Attempting login with: {username}");

                // Authenticate user
                var user = userRepo.AuthenticateUser(username, password);

                if (user != null)
                {
                    // Success!
                    System.Diagnostics.Debug.WriteLine($"Login successful for: {user.FullName} ({user.Role})");

                    // Set current user
                    CurrentUser.LoggedInUser = user;

                    // Show success message
                    MessageBox.Show(
                        $"✅ Welcome, {user.FullName}!\n\nRole: {user.Role}",
                        "Login Successful",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Show main window
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    // Close login window
                    this.Close();
                }
                else
                {
                    // Failed
                    System.Diagnostics.Debug.WriteLine("Login failed - Invalid credentials");

                    TxtError.Text = "❌ Invalid username or password!\n\nPlease check the credentials below.";
                    TxtPassword.Password = "";
                    TxtUsername.Focus();
                    TxtUsername.SelectAll();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");

                TxtError.Text = "❌ Login Error!";
                MessageBox.Show(
                    $"Error during login:\n\n{ex.Message}\n\n" +
                    $"Stack Trace:\n{ex.StackTrace}",
                    "Login Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                // Re-enable login button
                BtnLogin.IsEnabled = true;
                BtnLogin.Content = "🔐 LOGIN";
                BtnLogin.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#4CAF50");
            }
        }
    }
}