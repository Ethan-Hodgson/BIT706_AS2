using Assignment2.App.BusinessLayer;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for CustomerEditorWindow.xaml
    /// </summary>
    public partial class CustomerEditorWindow : Window
    {
        private readonly VetClinicService vetClinicService;
        private Customer? customer;

        public CustomerEditorWindow(VetClinicService vetClinicService)
        {
            InitializeComponent();
            this.vetClinicService = vetClinicService;
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

        private bool AddNewCustomer()
        {
            var newCustomer = new Customer
            {
                FirstName = firstName.Text,
                Surname = surname.Text,
                PhoneNumber = phoneNumber.Text,
                Address = address.Text
            };

            if (!newCustomer.CheckIfValid())
            {
                MessageBox.Show("Cannot save customer - some information is missing",
                                "Save error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            vetClinicService.CreateCustomer(newCustomer);
            return true;
        }

        private bool UpdateCustomer()
        {
            Customer!.FirstName = firstName.Text;
            Customer.Surname = surname.Text;
            Customer.PhoneNumber = phoneNumber.Text;
            Customer.Address = address.Text;

            if (!Customer.CheckIfValid())
            {
                MessageBox.Show("Cannot save customer - some information is missing",
                                "Save error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            vetClinicService.UpdateCustomer(Customer);
            return true;
        }
    }
}
