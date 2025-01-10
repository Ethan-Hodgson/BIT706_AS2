using Assignment2.App.BusinessLayer;
using System.Linq;
using System.Windows;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for AnimalEditorWindow.xaml
    /// </summary>
    public partial class AnimalEditorWindow : Window
    {
        private readonly VetClinicService vetClinicService;
        private Animal? animal;
        private Customer? customer;

        public AnimalEditorWindow(VetClinicService vetClinicService)
        {
            InitializeComponent();
            this.vetClinicService = vetClinicService;
        }

        public Animal? Animal
        {
            get => animal;
            set
            {
                customer = null;
                animal = value;
                animalName.Text = animal?.Name ?? string.Empty;
                type.Text = animal?.Type ?? string.Empty;
                sex.Text = animal?.Sex ?? string.Empty;
                breed.Text = animal?.Breed ?? string.Empty;
                owner.Text = string.Empty;

                if (animal != null)
                {
                    // If you need the current owner:
                    // You'd do something like:
                    //     customer = vetClinicService.GetCustomerById(animal.OwnerId);
                    // if you added that to VetClinicService.  
                    // Then:
                    //     owner.Text = customer?.ToString() ?? string.Empty;
                    //
                    // Otherwise, if you're not displaying the current owner, skip this.
                }
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnFindCustomer(object sender, RoutedEventArgs e)
        {
            // Open the "SearchForCustomerWindow" that also uses VetClinicService:
            var window = new SearchForCustomerWindow(vetClinicService)
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
            };
            if (window.ShowDialog() == true)
            {
                customer = window.Customer;
                owner.Text = customer?.ToString() ?? string.Empty;
            }
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if (Animal != null)
            {
                if (UpdateAnimal()) Close();
            }
            else
            {
                if (AddNewAnimal()) Close();
            }
        }

        private bool AddNewAnimal()
        {
            var newAnimal = new Animal
            {
                Name = animalName.Text,
                Type = type.Text,
                Breed = breed.Text,
                Sex = sex.Text,
                OwnerId = customer?.Id ?? 0
            };

            if (!newAnimal.CheckIfValid())
            {
                MessageBox.Show("Cannot save animal - some information is missing",
                                "Save error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            vetClinicService.CreateAnimal(newAnimal);
            return true;
        }

        private bool UpdateAnimal()
        {
            // We know Animal != null here
            Animal!.Name = animalName.Text;
            Animal.Type = type.Text;
            Animal.Breed = breed.Text;
            Animal.Sex = sex.Text;
            Animal.OwnerId = customer?.Id ?? 0;

            if (!Animal.CheckIfValid())
            {
                MessageBox.Show("Cannot save animal - some information is missing",
                                "Save error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }

            vetClinicService.UpdateAnimal(Animal);
            return true;
        }
    }
}
