using Assignment2.App.BusinessLayer;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for CustomerEditorWindow.xaml
    /// </summary>
    public partial class CustomerEditorWindow : Window
    {
        private readonly Store dataStore;
        private Customer? customer;

        public CustomerEditorWindow(Store dataStore)
        {
            InitializeComponent();
            this.dataStore = dataStore;
        }

        public Customer? Customer
        {
            get => customer;
            set
            {
                customer = value;
                firstName.Text = customer?.FirstName ?? string.Empty;
                surname.Text = customer?.Surname ?? string.Empty;
                phoneNumber.Text = customer?.PhoneNumber ?? string.Empty;
                address.Text = customer?.Address ?? string.Empty;
            }
        }

        private bool AddNewCustomer()
        {
            var customer = dataStore.AddCustomer();
            customer.FirstName = firstName.Text;
            customer.Surname = surname.Text;
            customer.PhoneNumber = phoneNumber.Text;
            customer.Address = address.Text;
            if (!customer.CheckIfValid())
            {
                MessageBox.Show(
                    "Cannot save customer - some information is missing",
                    "Save error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            dataStore.Customers.Add(customer);
            dataStore.Save("data");
            return true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (Customer != null)
            {
                if (UpdateCustomer()) Close();
            }
            else
            {
                if (AddNewCustomer()) Close();
            }
        }

        private bool UpdateCustomer()
        {
            Customer!.FirstName = firstName.Text;
            Customer.Surname = surname.Text;
            Customer.PhoneNumber = phoneNumber.Text;
            Customer.Address = address.Text;
            if (!Customer.CheckIfValid())
            {
                MessageBox.Show(
                    "Cannot save customer - some information is missing",
                    "Save error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            dataStore.Save("data");
            return true;
        }
    }
}