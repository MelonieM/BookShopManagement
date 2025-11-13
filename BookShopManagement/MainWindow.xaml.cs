using System;
using System.Windows;
using System.Windows.Threading;
using BookShopManagement.Data;

namespace BookShopManagement
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            // Test database connection on startup
            TestDatabaseConnection();

            InitializeTimer();

            // Load Dashboard by default
            LoadDashboard();
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    StatusText.Text = "Database connected successfully ✓";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed!\n\n{ex.Message}\n\nPlease check your connection string.",
                              "Database Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                StatusText.Text = "Database connection failed ✗";
            }
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTimeText.Text = DateTime.Now.ToString("MMM dd, yyyy | HH:mm:ss");
        }

        private void LoadDashboard()
        {
            try
            {
                MainFrame.Navigate(new Pages.DashboardPage());
                StatusText.Text = "Dashboard loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Dashboard: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDashboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.Navigate(new Pages.DashboardPage());
                StatusText.Text = "Dashboard loaded";
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
                StatusText.Text = "Book Management loaded";
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
                StatusText.Text = "Customer Management loaded";
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
                if (MainFrame == null)
                {
                    MessageBox.Show("MainFrame is null!", "Error");
                    return;
                }

                var salesPage = new Pages.SalesPage();
                MainFrame.Navigate(salesPage);

                if (StatusText != null)
                    StatusText.Text = "Sales Processing loaded";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\nStack: {ex.StackTrace}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Reports page will be added next!",
                          "Coming Soon",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
            StatusText.Text = "Reports - Coming Soon";
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

        public void UpdateStatus(string message)
        {
            StatusText.Text = message;
        }
    }
}
