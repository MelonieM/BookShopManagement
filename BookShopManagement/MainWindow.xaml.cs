using System;
using System.Windows;
using System.Windows.Threading;
using BookShopManagement.Data;
<<<<<<< HEAD
using BookShopManagement.Models;
=======
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707

namespace BookShopManagement
{
    public partial class MainWindow : Window
    {
<<<<<<< HEAD
        private DispatcherTimer? timer;
=======
        private DispatcherTimer timer;
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707

        public MainWindow()
        {
            InitializeComponent();

<<<<<<< HEAD
            // Check if user is logged in
            if (!CurrentUser.IsLoggedIn)
            {
                MessageBox.Show("Please login first", "Access Denied");
                Application.Current.Shutdown();
                return;
            }

            // Display user info
            if (CurrentUser.LoggedInUser != null)
            {
                TxtLoggedInUser.Text = CurrentUser.LoggedInUser.FullName;
                TxtUserRole.Text = CurrentUser.LoggedInUser.Role;
            }

=======
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            // Test database connection on startup
            TestDatabaseConnection();

            InitializeTimer();
<<<<<<< HEAD
            LoadDashboard();

            // Apply role-based permissions
            ApplyRolePermissions();
=======

            // Load Dashboard by default
            LoadDashboard();
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
<<<<<<< HEAD
                    if (StatusText != null)
                        StatusText.Text = "Database connected successfully ✓";
=======
                    StatusText.Text = "Database connected successfully ✓";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed!\n\n{ex.Message}\n\nPlease check your connection string.",
                              "Database Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
<<<<<<< HEAD
                if (StatusText != null)
                    StatusText.Text = "Database connection failed ✗";
=======
                StatusText.Text = "Database connection failed ✗";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

<<<<<<< HEAD
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (DateTimeText != null)
                DateTimeText.Text = DateTime.Now.ToString("MMM dd, yyyy | HH:mm:ss");
=======
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeText.Text = DateTime.Now.ToString("MMM dd, yyyy | HH:mm:ss");
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        }

        private void LoadDashboard()
        {
            try
            {
                MainFrame.Navigate(new Pages.DashboardPage());
<<<<<<< HEAD
                if (StatusText != null)
                    StatusText.Text = "Dashboard loaded - Welcome!";
=======
                StatusText.Text = "Dashboard loaded";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Dashboard: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

<<<<<<< HEAD
        private void ApplyRolePermissions()
        {
            if (CurrentUser.LoggedInUser == null) return;

            // Example: Cashiers have limited access
            if (CurrentUser.LoggedInUser.IsCashier)
            {
                // You can hide certain buttons for cashiers if needed
                // Example: BtnCustomers.Visibility = Visibility.Collapsed;
            }

            // Admins have full access - no restrictions
            if (CurrentUser.LoggedInUser.IsAdmin)
            {
                // You can add admin-only features here
            }
        }

=======
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.Navigate(new Pages.DashboardPage());
<<<<<<< HEAD
                if (StatusText != null)
                    StatusText.Text = "Dashboard loaded";
=======
                StatusText.Text = "Dashboard loaded";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Dashboard: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBooks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.Navigate(new Pages.BooksPage());
<<<<<<< HEAD
                if (StatusText != null)
                    StatusText.Text = "Book Management loaded";
=======
                StatusText.Text = "Book Management loaded";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Books page: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCustomers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.Navigate(new Pages.CustomersPage());
<<<<<<< HEAD
                if (StatusText != null)
                    StatusText.Text = "Customer Management loaded";
=======
                StatusText.Text = "Customer Management loaded";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Customers page: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
<<<<<<< HEAD
                MainFrame.Navigate(new Pages.SalesPage());
=======
                if (MainFrame == null)
                {
                    MessageBox.Show("MainFrame is null!", "Error");
                    return;
                }

                var salesPage = new Pages.SalesPage();
                MainFrame.Navigate(salesPage);

>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
                if (StatusText != null)
                    StatusText.Text = "Sales Processing loaded";
            }
            catch (Exception ex)
            {
<<<<<<< HEAD
                MessageBox.Show($"Error loading Sales page: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
=======
                MessageBox.Show($"Error: {ex.Message}\n\nStack: {ex.StackTrace}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
            }
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            try
            {
                MainFrame.Navigate(new Pages.ReportsPage());
                if (StatusText != null)
                    StatusText.Text = "Reports loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Reports page: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
=======
            MessageBox.Show("Reports page will be added next!",
                          "Coming Soon",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
            StatusText.Text = "Reports - Coming Soon";
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?",
                                       "Exit Confirmation",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

<<<<<<< HEAD
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Clear current user
                CurrentUser.Logout();

                // Show login window
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();

                // Close main window
                this.Close();
            }
        }

        public void UpdateStatus(string message)
        {
            if (StatusText != null)
                StatusText.Text = message;
        }
    }
}
=======
        public void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }
    }
}
>>>>>>> c7afd23eabfc4eef251d52bc3c33511c70bac707
