using System;
using System.Windows;
using BookShopManagement.Models;

namespace BookShopManagement.Windows
{
    public partial class AddEditBookWindow : Window
    {
        public Book Book { get; private set; }
        private bool isEditMode;

        public AddEditBookWindow()
        {
            InitializeComponent();
            isEditMode = false;
            Book = new Book();
            Title = "Add New Book";
        }

        public AddEditBookWindow(Book book)
        {
            InitializeComponent();
            isEditMode = true;
            Book = book;
            Title = "Edit Book";
            LoadBookData();
        }

        private void LoadBookData()
        {
            TxtISBN.Text = Book.ISBN;
            TxtTitle.Text = Book.Title;
            TxtAuthor.Text = Book.Author;
            TxtPrice.Text = Book.Price.ToString("F2");
            TxtStock.Text = Book.StockQuantity.ToString();
            TxtCategory.Text = Book.Category;
            TxtPublisher.Text = Book.Publisher;
            TxtYear.Text = Book.PublishedYear?.ToString() ?? "";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Book.ISBN = TxtISBN.Text.Trim();
                Book.Title = TxtTitle.Text.Trim();
                Book.Author = TxtAuthor.Text.Trim();
                Book.Price = decimal.Parse(TxtPrice.Text.Trim());
                Book.StockQuantity = int.Parse(TxtStock.Text.Trim());
                Book.Category = string.IsNullOrWhiteSpace(TxtCategory.Text) ? null : TxtCategory.Text.Trim();
                Book.Publisher = string.IsNullOrWhiteSpace(TxtPublisher.Text) ? null : TxtPublisher.Text.Trim();

                if (!string.IsNullOrWhiteSpace(TxtYear.Text))
                {
                    Book.PublishedYear = int.Parse(TxtYear.Text.Trim());
                }
                else
                {
                    Book.PublishedYear = null;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving book: {ex.Message}",
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

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(TxtISBN.Text))
            {
                MessageBox.Show("ISBN is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtISBN.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtTitle.Text))
            {
                MessageBox.Show("Title is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtAuthor.Text))
            {
                MessageBox.Show("Author is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtAuthor.Focus();
                return false;
            }

            if (!decimal.TryParse(TxtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price greater than 0.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtPrice.Focus();
                return false;
            }

            if (!int.TryParse(TxtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock quantity (0 or greater).", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtStock.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(TxtYear.Text))
            {
                if (!int.TryParse(TxtYear.Text, out int year) || year < 1000 || year > DateTime.Now.Year)
                {
                    MessageBox.Show($"Please enter a valid year (1000 - {DateTime.Now.Year}).", "Validation Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    TxtYear.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}