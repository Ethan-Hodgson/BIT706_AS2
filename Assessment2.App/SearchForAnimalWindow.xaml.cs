using Assignment2.App.BusinessLayer;
using System.Windows;
using System.Windows.Controls;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for SearchForAnimalWindow.xaml
    /// </summary>
    public partial class SearchForAnimalWindow : Window
    {
        private readonly Store dataStore;
        private Customer? customer;

        public SearchForAnimalWindow(Store dataStore)
        {
            InitializeComponent();
            this.dataStore = dataStore;
        }

        public Animal? Animal { get; private set; }

        public Customer? Customer
        {
            get => customer;
            set
            {
                customer = value;
                var animals = dataStore.FindAnimals(customer?.Id ?? 0);
                foreach (var animal in animals)
                {
                    searchResults.Items.Add(new ListBoxItem { Content = animal });
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