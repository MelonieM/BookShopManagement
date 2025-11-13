using System;
using System.Windows;
using BookShopManagement.Models;

namespace BookShopManagement.Windows
{
    public partial class AddEditCustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        //private bool isEditMode;

        public AddEditCustomerWindow()
        {
            InitializeComponent();
           // isEditMode = false;
            Customer = new Customer();
            Title = "Add New Customer";
        }

        public AddEditCustomerWindow(Customer customer)
        {
            InitializeComponent();
          //  isEditMode = true;
            Customer = customer;
            Title = "Edit Customer";
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            TxtName.Text = Customer.Name;
            TxtContact.Text = Customer.Contact;
            TxtEmail.Text = Customer.Email;
            TxtAddress.Text = Customer.Address;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Customer.Name = TxtName.Text.Trim();
                Customer.Contact = TxtContact.Text.Trim();
                Customer.Email = string.IsNullOrWhiteSpace(TxtEmail.Text) ? null : TxtEmail.Text.Trim();
                Customer.Address = string.IsNullOrWhiteSpace(TxtAddress.Text) ? null : TxtAddress.Text.Trim();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving customer: {ex.Message}",
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
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show("Name is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TxtContact.Text))
            {
                MessageBox.Show("Contact is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtContact.Focus();
                return false;
            }

            return true;
        }
    }
}