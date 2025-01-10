using Assignment2.App.BusinessLayer;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for SearchForAnimalWindow.xaml
    /// </summary>
    public partial class SearchForAnimalWindow : Window
    {
        private readonly VetClinicService vetClinicService;
        private Customer? customer;

        public SearchForAnimalWindow(VetClinicService vetClinicService)
        {
            InitializeComponent();
            this.vetClinicService = vetClinicService;
        }

        public Animal? Animal { get; private set; }

        public Customer? Customer
        {
            get => customer;
            set
            {
                customer = value;
                searchResults.Items.Clear();

                if (customer != null)
                {
                    // Use vetClinicService.GetAnimalsByOwner(...)
                    var animals = vetClinicService.GetAnimalsByOwner(customer.Id);

                    foreach (var animal in animals)
                    {
                        searchResults.Items.Add(new ListBoxItem { Content = animal });
                    }
                }
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnSelect(object sender, RoutedEventArgs e)
        {
            if (searchResults.SelectedItem == null) return;
            DialogResult = true;
            Animal = ((ListBoxItem)searchResults.SelectedItem).Content as Animal;
            Close();
        }
    }
}
