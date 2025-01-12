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
            // Create a fresh instance of the search window and view model
            var animalSearchVm = new SearchForAnimalViewModel(clinicService);
            var animalSearchWindow = new SearchForAnimalWindow(animalSearchVm)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Load animals initially
            animalSearchVm.LoadAnimals();

            // Display the search window
            if (animalSearchWindow.ShowDialog() == true)
            {
                var selectedAnimal = animalSearchVm.SelectedAnimal;
                if (selectedAnimal != null)
                {
                    // Create a fresh instance of the edit window and view model
                    var editAnimalVm = new EditAnimalViewModel(clinicService, selectedAnimal);

                    // Handle RequestClose from the edit window
                    editAnimalVm.RequestClose += shouldRefresh =>
                    {
                        // Close the edit window and handle refresh if needed
                        if (shouldRefresh)
                        {
                            // Recreate and display a new search window
                            var newSearchVm = new SearchForAnimalViewModel(clinicService);
                            var newSearchWindow = new SearchForAnimalWindow(newSearchVm)
                            {
                                Owner = this,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            };

                            newSearchVm.LoadAnimals(); // Refresh animals list
                            newSearchWindow.ShowDialog();
                        }
                    };

                    // Display the edit window
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
