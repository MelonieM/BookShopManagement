using System;
using System.Windows;
using System.Windows.Controls;
using BookShopManagement.Data;
using BookShopManagement.Models;
using BookShopManagement.Windows;

namespace BookShopManagement.Pages
{
    public partial class CustomersPage : Page
    {
        private CustomerRepository customerRepo = new CustomerRepository();
        private SalesRepository salesRepo = new SalesRepository();

        public CustomersPage()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                var customers = customerRepo.GetAllCustomers();
                CustomersDataGrid.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditCustomerWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    if (customerRepo.AddCustomer(addWindow.Customer))
                    {
                        MessageBox.Show("Customer added successfully!",
                                      "Success",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        LoadCustomers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer.",
                                      "Error",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding customer: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to edit.",
                              "Edit Customer",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedCustomer = (Customer)CustomersDataGrid.SelectedItem;
            var editWindow = new AddEditCustomerWindow(selectedCustomer);

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    if (customerRepo.UpdateCustomer(editWindow.Customer))
                    {
                        MessageBox.Show("Customer updated successfully!",
                                      "Success",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        LoadCustomers();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update customer.",
                                      "Error",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating customer: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to delete.",
                              "Delete Customer",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedCustomer = (Customer)CustomersDataGrid.SelectedItem;

            MessageBox.Show("Customer deletion is not implemented.\n\nThis would require handling foreign key constraints with the Sales table.\n\nIn a production system, you would either:\n1. Prevent deletion if customer has purchase history\n2. Implement soft delete (mark as inactive)\n3. Delete all related sales first",
                          "Not Implemented",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (CustomersDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to view purchase history.",
                              "Purchase History",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedCustomer = (Customer)CustomersDataGrid.SelectedItem;

            try
            {
                var sales = salesRepo.GetCustomerPurchaseHistory(selectedCustomer.CustomerID);

                string message = $"Purchase History for: {selectedCustomer.Name}\n";
                message += $"Email: {selectedCustomer.Email}\n";
                message += $"Contact: {selectedCustomer.Contact}\n\n";

                if (sales.Count == 0)
                {
                    message += "No purchases found.";
                }
                else
                {
                    decimal totalSpent = 0;
                    message += "Recent Purchases:\n";
                    message += new string('-', 50) + "\n";

                    foreach (var sale in sales)
                    {
                        message += $"Date: {sale.SaleDate:yyyy-MM-dd HH:mm}\n";
                        message += $"Amount: ${sale.FinalAmount:F2}";
                        if (sale.DiscountPercent > 0)
                        {
                            message += $" (Discount: {sale.DiscountPercent}%)";
                        }
                        message += $"\nPayment: {sale.PaymentMethod ?? "N/A"}\n\n";
                        totalSpent += sale.FinalAmount;
                    }

                    message += new string('-', 50) + "\n";
                    message += $"Total Purchases: {sales.Count}\n";
                    message += $"Total Spent: ${totalSpent:F2}";
                }

                MessageBox.Show(message,
                              "Purchase History",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading purchase history: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }
    }
}