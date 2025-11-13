using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BookShopManagement.Data;
using BookShopManagement.Models;

namespace BookShopManagement.Pages
{
    public partial class SalesPage : Page
    {
        private List<CartItem> cart = new List<CartItem>();

        public SalesPage()
        {
            InitializeComponent();
            LoadBooks();
            LoadCustomers();
            UpdateCart();
        }

        private void LoadBooks()
        {
            try
            {
                var repo = new BookRepository();
                BooksGrid.ItemsSource = repo.GetAllBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var repo = new CustomerRepository();
                CustomerComboBox.ItemsSource = repo.GetAllCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var repo = new BookRepository();
                var search = SearchBox.Text.Trim();
                BooksGrid.ItemsSource = string.IsNullOrEmpty(search)
                    ? repo.GetAllBooks()
                    : repo.SearchBooks(search);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BooksGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BooksGrid.SelectedItem == null) return;

            var book = (Book)BooksGrid.SelectedItem;

            if (book.StockQuantity <= 0)
            {
                MessageBox.Show("Out of stock!");
                return;
            }

            var existing = cart.FirstOrDefault(c => c.BookID == book.BookID);
            if (existing != null)
            {
                if (existing.Quantity >= book.StockQuantity)
                {
                    MessageBox.Show($"Only {book.StockQuantity} in stock!");
                    return;
                }
                existing.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    BookID = book.BookID,
                    Title = book.Title,
                    Price = book.Price,
                    Quantity = 1,
                    MaxStock = book.StockQuantity
                });
            }

            UpdateCart();
        }

        private void UpdateCart()
        {
            CartPanel.Children.Clear();

            if (cart.Count == 0)
            {
                CartPanel.Children.Add(new TextBlock
                {
                    Text = "Cart is empty\nDouble-click a book to add",
                    Foreground = Brushes.Gray,
                    FontStyle = FontStyles.Italic,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(20)
                });
                TotalText.Text = "$0.00";
                return;
            }

            foreach (var item in cart)
            {
                var border = new Border
                {
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Padding = new Thickness(10),
                    Margin = new Thickness(5)
                };

                var stack = new StackPanel();

                // Title
                stack.Children.Add(new TextBlock
                {
                    Text = item.Title,
                    FontWeight = FontWeights.Bold,
                    TextTrimming = TextTrimming.CharacterEllipsis
                });

                // Price and controls
                var controlStack = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 0)
                };

                var btnMinus = new Button
                {
                    Content = "−",
                    Width = 25,
                    Height = 25,
                    Tag = item
                };
                btnMinus.Click += (s, e) =>
                {
                    if (item.Quantity > 1)
                        item.Quantity--;
                    else
                        cart.Remove(item);
                    UpdateCart();
                };
                controlStack.Children.Add(btnMinus);

                controlStack.Children.Add(new TextBlock
                {
                    Text = $" {item.Quantity} ",
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.Bold
                });

                var btnPlus = new Button
                {
                    Content = "+",
                    Width = 25,
                    Height = 25,
                    Tag = item
                };
                btnPlus.Click += (s, e) =>
                {
                    if (item.Quantity < item.MaxStock)
                        item.Quantity++;
                    else
                        MessageBox.Show($"Only {item.MaxStock} in stock!");
                    UpdateCart();
                };
                controlStack.Children.Add(btnPlus);

                controlStack.Children.Add(new TextBlock
                {
                    Text = $" × ${item.Price:F2} = ${item.Subtotal:F2}",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                });

                var btnRemove = new Button
                {
                    Content = "🗑️",
                    Margin = new Thickness(10, 0, 0, 0),
                    Tag = item,
                    Background = Brushes.Red,
                    Foreground = Brushes.White
                };
                btnRemove.Click += (s, e) =>
                {
                    cart.Remove(item);
                    UpdateCart();
                };
                controlStack.Children.Add(btnRemove);

                stack.Children.Add(controlStack);
                border.Child = stack;
                CartPanel.Children.Add(border);
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal subtotal = cart.Sum(c => c.Subtotal);
            decimal discount = 0;

            if (decimal.TryParse(DiscountBox.Text, out decimal discountPercent))
            {
                discount = subtotal * (discountPercent / 100);
            }

            decimal total = subtotal - discount;
            TotalText.Text = $"${total:F2}";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (cart.Count == 0) return;

            if (MessageBox.Show("Clear cart?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                cart.Clear();
                UpdateCart();
            }
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            try
            {
                var sale = new Sale
                {
                    SaleDate = DateTime.Now,
                    CustomerID = CustomerComboBox.SelectedValue as int?,
                    PaymentMethod = "Cash"
                };

                decimal subtotal = cart.Sum(c => c.Subtotal);
                decimal.TryParse(DiscountBox.Text, out decimal discountPercent);

                sale.TotalAmount = subtotal;
                sale.DiscountPercent = discountPercent;
                sale.FinalAmount = subtotal - (subtotal * discountPercent / 100);

                foreach (var item in cart)
                {
                    sale.Items.Add(new SaleItem
                    {
                        BookID = item.BookID,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        Subtotal = item.Subtotal,
                        Book = new Book { BookID = item.BookID, Title = item.Title }
                    });
                }

                var repo = new SalesRepository();
                int saleID = repo.CreateSale(sale);

                MessageBox.Show($"Sale completed!\nID: {saleID}\nTotal: ${sale.FinalAmount:F2}",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                cart.Clear();
                CustomerComboBox.SelectedIndex = -1;
                DiscountBox.Text = "0";
                UpdateCart();
                LoadBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n\n{ex.StackTrace}", "Error");
            }
        }

        // Helper class
        private class CartItem
        {
            public int BookID { get; set; }
            public string Title { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int MaxStock { get; set; }
            public decimal Subtotal => Price * Quantity;
        }
    }
}