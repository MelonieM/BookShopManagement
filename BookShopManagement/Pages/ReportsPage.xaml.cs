using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookShopManagement.Data;

namespace BookShopManagement.Pages
{
    public partial class ReportsPage : Page
    {
        private ReportsRepository reportsRepo = new ReportsRepository();
        private SalesRepository salesRepo = new SalesRepository();
        private BookRepository bookRepo = new BookRepository();
        private CustomerRepository customerRepo = new CustomerRepository();

        public ReportsPage()
        {
            InitializeComponent();
        }

        private void BtnTodaySales_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                DateTime endOfDay = today.AddDays(1).AddSeconds(-1);

                var report = reportsRepo.GetSalesReport(today, endOfDay);
                var sales = salesRepo.GetSalesByDateRange(today, endOfDay);

                ReportTitle.Text = $"📅 Today's Sales Report - {DateTime.Today:yyyy-MM-dd}";

                string content = $"═══════════════════════════════════════════\n";
                content += $"SALES SUMMARY FOR TODAY\n";
                content += $"═══════════════════════════════════════════\n\n";
                content += $"Total Sales: {report.TotalSales}\n";
                content += $"Total Books Sold: {report.TotalBooksSold}\n";
                content += $"Total Revenue: ${report.TotalRevenue:F2}\n\n";

                if (sales.Count > 0)
                {
                    content += $"───────────────────────────────────────────\n";
                    content += $"SALES DETAILS:\n";
                    content += $"───────────────────────────────────────────\n\n";

                    foreach (var sale in sales)
                    {
                        content += $"Sale ID: {sale.SaleID}\n";
                        content += $"Time: {sale.SaleDate:HH:mm:ss}\n";
                        content += $"Amount: ${sale.FinalAmount:F2}\n";
                        if (sale.DiscountPercent > 0)
                            content += $"Discount: {sale.DiscountPercent}%\n";
                        content += $"Payment: {sale.PaymentMethod ?? "N/A"}\n";
                        content += $"\n";
                    }
                }
                else
                {
                    content += "No sales recorded today.\n";
                }

                ReportContent.Text = content;
                ReportDataGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnDateRangeSales_Click(object sender, RoutedEventArgs e)
        {
            // Create a simple date range dialog
            var dialog = new Window
            {
                Title = "Select Date Range",
                Width = 350,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Window.GetWindow(this)
            };

            var stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock { Text = "Start Date:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 5) });
            var startDate = new DatePicker { SelectedDate = DateTime.Today.AddMonths(-1), Margin = new Thickness(0, 0, 0, 15) };
            stack.Children.Add(startDate);

            stack.Children.Add(new TextBlock { Text = "End Date:", FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 5) });
            var endDate = new DatePicker { SelectedDate = DateTime.Today, Margin = new Thickness(0, 0, 0, 15) };
            stack.Children.Add(endDate);

            var btnGenerate = new Button
            {
                Content = "Generate Report",
                Height = 35,
                Background = System.Windows.Media.Brushes.Green,
                Foreground = System.Windows.Media.Brushes.White,
                FontWeight = FontWeights.SemiBold
            };
            btnGenerate.Click += (s, ev) => dialog.DialogResult = true;
            stack.Children.Add(btnGenerate);

            dialog.Content = stack;

            if (dialog.ShowDialog() == true && startDate.SelectedDate.HasValue && endDate.SelectedDate.HasValue)
            {
                try
                {
                    DateTime start = startDate.SelectedDate.Value;
                    DateTime end = endDate.SelectedDate.Value.AddDays(1).AddSeconds(-1);

                    var report = reportsRepo.GetSalesReport(start, end);
                    var sales = salesRepo.GetSalesByDateRange(start, end);

                    ReportTitle.Text = $"📊 Sales Report: {start:yyyy-MM-dd} to {endDate.SelectedDate.Value:yyyy-MM-dd}";

                    string content = $"═══════════════════════════════════════════\n";
                    content += $"SALES REPORT\n";
                    content += $"Period: {start:yyyy-MM-dd} to {endDate.SelectedDate.Value:yyyy-MM-dd}\n";
                    content += $"═══════════════════════════════════════════\n\n";
                    content += $"Total Sales: {report.TotalSales}\n";
                    content += $"Total Books Sold: {report.TotalBooksSold}\n";
                    content += $"Total Revenue: ${report.TotalRevenue:F2}\n\n";

                    if (sales.Count > 0)
                    {
                        content += $"───────────────────────────────────────────\n";
                        content += $"SALES BREAKDOWN:\n";
                        content += $"───────────────────────────────────────────\n\n";

                        foreach (var sale in sales)
                        {
                            content += $"Sale ID: {sale.SaleID} | Date: {sale.SaleDate:yyyy-MM-dd HH:mm} | Amount: ${sale.FinalAmount:F2}\n";
                        }
                    }
                    else
                    {
                        content += "No sales found in this date range.\n";
                    }

                    ReportContent.Text = content;
                    ReportDataGrid.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error");
                }
            }
        }

        private void BtnLowStock_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lowStockBooks = reportsRepo.GetLowStockBooks(10);

                ReportTitle.Text = "⚠️ Low Stock Alert - Books Below 10 Units";

                string content = $"═══════════════════════════════════════════\n";
                content += $"LOW STOCK ALERT\n";
                content += $"Threshold: 10 units\n";
                content += $"═══════════════════════════════════════════\n\n";

                if (lowStockBooks.Count > 0)
                {
                    content += $"Found {lowStockBooks.Count} book(s) with low stock:\n\n";
                    content += $"───────────────────────────────────────────\n\n";

                    foreach (var book in lowStockBooks)
                    {
                        string status = book.StockQuantity == 0 ? "OUT OF STOCK!" : $"{book.StockQuantity} units";
                        content += $"[{book.ISBN}] {book.Title}\n";
                        content += $"Author: {book.Author}\n";
                        content += $"Stock: {status}\n";
                        content += $"Price: ${book.Price:F2}\n\n";
                    }

                    content += $"───────────────────────────────────────────\n";
                    content += $"⚠️ ACTION REQUIRED: Restock these items!\n";
                }
                else
                {
                    content += "✓ All books are adequately stocked!\n";
                    content += "No items below threshold.\n";
                }

                ReportContent.Text = content;
                ReportDataGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void BtnTopSelling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var topBooks = reportsRepo.GetTopSellingBooks(10);

                ReportTitle.Text = "🏆 Top 10 Best Selling Books";

                string content = $"═══════════════════════════════════════════\n";
                content += $"TOP SELLING BOOKS (ALL TIME)\n";
                content += $"═══════════════════════════════════════════\n\n";

                if (topBooks.Count > 0)
                {
                    int rank = 1;
                    foreach (var book in topBooks)
                    {
                        string medal = rank == 1 ? "🥇" : rank == 2 ? "🥈" : rank == 3 ? "🥉" : $"{rank}.";
                        content += $"{medal} {book.Key}\n";
                        content += $"   Total Sold: {book.Value} copies\n\n";
                        rank++;
                    }
                }
                else
                {
                    content += "No sales data available yet.\n";
                }

                ReportContent.Text = content;
                ReportDataGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }
    }
}