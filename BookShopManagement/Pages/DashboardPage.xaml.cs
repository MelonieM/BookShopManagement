using System;
using System.Windows;
using System.Windows.Controls;
using BookShopManagement.Data;

namespace BookShopManagement.Pages
{
    public partial class DashboardPage : Page
    {
        private BookRepository bookRepo = new BookRepository();
        private CustomerRepository customerRepo = new CustomerRepository();
        private SalesRepository salesRepo = new SalesRepository();
        private ReportsRepository reportsRepo = new ReportsRepository();

        public DashboardPage()
        {
            InitializeComponent();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                // Load statistics
                var allBooks = bookRepo.GetAllBooks();
                TotalBooksText.Text = allBooks.Count.ToString();

                var allCustomers = customerRepo.GetAllCustomers();
                TotalCustomersText.Text = allCustomers.Count.ToString();

                var todayRevenue = reportsRepo.GetTotalRevenueToday();
                TodayRevenueText.Text = $"${todayRevenue:F2}";

                var lowStockBooks = reportsRepo.GetLowStockBooks(10);
                LowStockText.Text = lowStockBooks.Count.ToString();

                // Load recent sales
                DateTime today = DateTime.Today;
                DateTime endOfDay = today.AddDays(1).AddSeconds(-1);
                var todaysSales = salesRepo.GetSalesByDateRange(today, endOfDay);
                RecentSalesGrid.ItemsSource = todaysSales;

                // Load low stock books
                LowStockGrid.ItemsSource = lowStockBooks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadDashboardData();
            MessageBox.Show("Dashboard refreshed!", "Success",
                          MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}