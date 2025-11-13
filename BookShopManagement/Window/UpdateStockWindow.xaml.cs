using System;
using System.Windows;
using System.Windows.Media;
using BookShopManagement.Models;
using BookShopManagement.Data;

namespace BookShopManagement.Windows
{
    public partial class UpdateStockWindow : Window
    {
        private Book book;
        private BookRepository bookRepo = new BookRepository();

        public UpdateStockWindow(Book selectedBook)
        {
            InitializeComponent();
            book = selectedBook;
            LoadBookInfo();
        }

        private void LoadBookInfo()
        {
            TxtBookTitle.Text = book.Title;
            TxtBookISBN.Text = $"ISBN: {book.ISBN}";
            TxtCurrentStock.Text = $"Current Stock: {book.StockQuantity}";
        }

        private void TxtQuantity_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (int.TryParse(TxtQuantity.Text, out int quantity))
            {
                int newStock = book.StockQuantity + quantity;
                TxtNewStock.Text = $"New Stock: {newStock}";

                if (newStock < 0)
                {
                    TxtNewStock.Foreground = Brushes.Red;
                }
                else
                {
                    TxtNewStock.Foreground = Brushes.Green;
                }
            }
            else
            {
                TxtNewStock.Text = "New Stock: --";
                TxtNewStock.Foreground = Brushes.Gray;
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtQuantity.Text, out int quantity))
            {
                MessageBox.Show("Please enter a valid quantity.",
                              "Validation Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            int newStock = book.StockQuantity + quantity;
            if (newStock < 0)
            {
                MessageBox.Show("Stock cannot be negative!",
                              "Validation Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (bookRepo.UpdateStock(book.BookID, quantity))
                {
                    MessageBox.Show($"Stock updated successfully!\nNew Stock: {newStock}",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("Failed to update stock.",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating stock: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}