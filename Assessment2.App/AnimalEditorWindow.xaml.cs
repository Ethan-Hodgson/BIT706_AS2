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
        private readonly Store dataStore;
        private Animal? animal;
        private Customer? customer;

        public AnimalEditorWindow(Store dataStore)
        {
            InitializeComponent();
            this.dataStore = dataStore;
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

                if (animal == null) return;
                customer = dataStore.Customers.FirstOrDefault(c => c.Id == animal.OwnerId);
                owner.Text = customer?.ToString() ?? string.Empty;
            }
        }

        private bool AddNewAnimal()
        {
            var animal = dataStore.AddAnimal();
            animal.Name = animalName.Text;
            animal.Type = type.Text;
            animal.Breed = breed.Text;
            animal.Sex = sex.Text;
            animal.OwnerId = customer?.Id ?? 0;
            if (!animal.CheckIfValid())
            {
                MessageBox.Show(
                    "Cannot save animal - some information is missing",
                    "Save error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            dataStore.Animals.Add(animal);
            dataStore.Save("data");
            return true;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnFindCustomer(object sender, RoutedEventArgs e)
        {
            var window = new SearchForCustomerWindow(dataStore)
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

        private bool UpdateAnimal()
        {
            Animal!.Name = animalName.Text;
            Animal.Type = type.Text;
            Animal.Breed = breed.Text;
            Animal.Sex = sex.Text;
            Animal.OwnerId = customer?.Id ?? 0;
            if (!Animal.CheckIfValid())
            {
                MessageBox.Show(
                    "Cannot save animal - some information is missing",
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