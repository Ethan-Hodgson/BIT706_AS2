using Assignment2.App.BusinessLayer;
using Assignment2.App.ViewModels;
using Assignment2.App.Views;
using System;
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
            var customerSearchVm = new SearchForCustomerViewModel(clinicService);
            var customerSearchWindow = new SearchForCustomerWindow(customerSearchVm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (customerSearchWindow.ShowDialog() == true && customerSearchVm.SelectedCustomer != null)
            {
                var selectedCustomer = customerSearchVm.SelectedCustomer;
                var editVm = new EditCustomerViewModel(clinicService, selectedCustomer.Id);
                var editWindow = new EditCustomerWindow(editVm)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                editWindow.ShowDialog();
            }
        }


        private void OnAddAnimal(object sender, RoutedEventArgs e)
        {
            var addAnimalVm = new AddAnimalViewModel(clinicService, this);
            var addAnimalWindow = new AddAnimalWindow(addAnimalVm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            addAnimalWindow.ShowDialog();
        }

        private void OnEditAnimal(object sender, RoutedEventArgs e)
        {
            var animalSearchVm = new SearchForAnimalViewModel(clinicService);
            var animalSearchWindow = new SearchForAnimalWindow(animalSearchVm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Assuming OwnerId is provided dynamically; replace 0 with the actual ownerId
            animalSearchWindow.LoadAnimals(0);

            if (animalSearchWindow.ShowDialog() == true)
            {
                var selectedAnimal = animalSearchVm.SelectedAnimal;
                if (selectedAnimal != null)
                {
                    Console.WriteLine($"Editing Animal: {selectedAnimal.Name}");
                    var editAnimalVm = new EditAnimalViewModel(clinicService, selectedAnimal);
                    var editAnimalWindow = new EditAnimalWindow(editAnimalVm)
                    {
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    editAnimalWindow.ShowDialog();
                }
            }
        }






        private void OnExitApplication(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
