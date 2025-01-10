using Assignment2.App.BusinessLayer;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Replaces "private Store dataStore = new();" with a VetClinicService
        private VetClinicService clinicService;

        public MainWindow()
        {
            InitializeComponent();

            // 1) Create the CSV repositories
            var animalRepo = new CsvAnimalRepository("data/animals.csv");
            var customerRepo = new CsvCustomerRepository("data/customers.csv");

            // 2) Construct the VetClinicService
            clinicService = new VetClinicService(animalRepo, customerRepo);

            // No need for dataStore.Load("data") if each CSV repository loads automatically in its constructor.
        }

        private void EditAnimal(Animal? animal)
        {
            // Assuming AnimalEditorWindow has been refactored to take a VetClinicService 
            // instead of Store in its constructor.
            var window = new AnimalEditorWindow(clinicService)
            {
                Animal = animal,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };
            window.ShowDialog();
        }

        private void EditCustomer(Customer? customer)
        {
            // Assuming CustomerEditorWindow also takes a VetClinicService
            var window = new CustomerEditorWindow(clinicService)
            {
                Customer = customer,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };
            window.ShowDialog();
        }

        private void OnAddAnimal(object sender, RoutedEventArgs e)
        {
            EditAnimal(null);
        }

        private void OnAddCustomer(object sender, RoutedEventArgs e)
        {
            EditCustomer(null);
        }

        private void OnEditAnimal(object sender, RoutedEventArgs e)
        {
            // Assuming SearchForCustomerWindow has also been refactored
            // to accept VetClinicService in its constructor
            var customerSearch = new SearchForCustomerWindow(clinicService)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            if (customerSearch.ShowDialog() != true) return;

            // Then SearchForAnimalWindow also uses VetClinicService
            var animalSearch = new SearchForAnimalWindow(clinicService)
            {
                Customer = customerSearch.Customer,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            if (animalSearch.ShowDialog() == true)
            {
                EditAnimal(animalSearch.Animal);
            }
        }

        private void OnEditCustomer(object sender, RoutedEventArgs e)
        {
            // Similarly refactored for VetClinicService
            var customerSearch = new SearchForCustomerWindow(clinicService)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            if (customerSearch.ShowDialog() == true)
            {
                EditCustomer(customerSearch.Customer);
            }
        }

        private void OnExitApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
