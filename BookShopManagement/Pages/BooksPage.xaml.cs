using System;
using System.Windows;
using System.Windows.Controls;
using BookShopManagement.Data;
using BookShopManagement.Models;
using BookShopManagement.Windows;

namespace BookShopManagement.Pages
{
    public partial class BooksPage : Page
    {
        private BookRepository bookRepo = new BookRepository();

        public BooksPage()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            try
            {
                var books = bookRepo.GetAllBooks();
                BooksDataGrid.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading books: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.",
                              "Search",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
                return;
            }

            try
            {
                var books = bookRepo.SearchBooks(searchTerm);
                BooksDataGrid.ItemsSource = books;

                if (books.Count == 0)
                {
                    MessageBox.Show("No books found matching your search.",
                                  "Search Results",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching books: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            LoadBooks();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditBookWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    if (bookRepo.AddBook(addWindow.Book))
                    {
                        MessageBox.Show("Book added successfully!",
                                      "Success",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add book.",
                                      "Error",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding book: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to edit.",
                              "Edit Book",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedBook = (Book)BooksDataGrid.SelectedItem;
            var editWindow = new AddEditBookWindow(selectedBook);

            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    if (bookRepo.UpdateBook(editWindow.Book))
                    {
                        MessageBox.Show("Book updated successfully!",
                                      "Success",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update book.",
                                      "Error",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating book: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to delete.",
                              "Delete Book",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedBook = (Book)BooksDataGrid.SelectedItem;
            var result = MessageBox.Show($"Are you sure you want to delete:\n\n{selectedBook.Title}?",
                                       "Confirm Delete",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (bookRepo.DeleteBook(selectedBook.BookID))
                    {
                        MessageBox.Show("Book deleted successfully!",
                                      "Success",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                        LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete book.",
                                      "Error",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting book: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private void BtnUpdateStock_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Please select a book to update stock.",
                              "Update Stock",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }

            var selectedBook = (Book)BooksDataGrid.SelectedItem;
            var updateWindow = new UpdateStockWindow(selectedBook);

            if (updateWindow.ShowDialog() == true)
            {
                LoadBooks();
            }
        }
    }
}