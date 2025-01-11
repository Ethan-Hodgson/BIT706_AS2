using Assignment2.App.BusinessLayer;
using Assignment2.App.ViewModels;
using Assignment2.App.Views;
using Assignment2.App;
using System.Windows;

namespace Assignment2.App
{

    public partial class MainWindow : Window
    {
        private readonly VetClinicService clinicService;

        public MainWindow()
        {
            InitializeComponent();
            var animalRepo = new CsvAnimalRepository("data/animals.csv");
            var customerRepo = new CsvCustomerRepository("data/customers.csv");
            clinicService = new VetClinicService(animalRepo, customerRepo);
        }

        private void OnAddCustomer(object sender, RoutedEventArgs e)
        {
            var addCustomerVm = new AddCustomerViewModel(clinicService);
            var addCustomerWindow = new AddCustomerWindow(addCustomerVm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            addCustomerWindow.ShowDialog();
        }

        private void OnEditCustomer(object sender, RoutedEventArgs e)
        {
            var customerSearch = new SearchForCustomerWindow(clinicService)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (customerSearch.ShowDialog() == true)
            {
                var selectedCustomer = customerSearch.Customer;
                if (selectedCustomer != null)
                {
                    var editVm = new EditCustomerViewModel(clinicService, selectedCustomer.Id);
                    var editWindow = new EditCustomerWindow(editVm) // Ensure this matches the constructor
                    {
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    editWindow.ShowDialog();
                }
            }
        }





        private void OnAddAnimal(object sender, RoutedEventArgs e)
        {
            // Logic for adding an animal
        }

        private void OnEditAnimal(object sender, RoutedEventArgs e)
        {
            // Logic for editing an animal
        }

        private void OnExitApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
