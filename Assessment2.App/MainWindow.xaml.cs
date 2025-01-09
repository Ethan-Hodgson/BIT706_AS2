using Assignment2.App.BusinessLayer;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Store dataStore = new();

        public MainWindow()
        {
            InitializeComponent();
            dataStore.Load("data");
        }

        private void EditAnimal(Animal? animal)
        {
            var window = new AnimalEditorWindow(dataStore)
            {
                Animal = animal,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };
            window.ShowDialog();
        }

        private void EditCustomer(Customer? customer)
        {
            var window = new CustomerEditorWindow(dataStore)
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
            var customerSearch = new SearchForCustomerWindow(dataStore)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            if (customerSearch.ShowDialog() != true) return;
            var animalSearch = new SearchForAnimalWindow(dataStore)
            {
                Customer = customerSearch.Customer,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };

            if (animalSearch.ShowDialog() == true) EditAnimal(animalSearch.Animal);
        }

        private void OnEditCustomer(object sender, RoutedEventArgs e)
        {
            var customerSearch = new SearchForCustomerWindow(dataStore)
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