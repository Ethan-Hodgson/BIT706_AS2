using Assignment2.App.BusinessLayer;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Assignment2.App
{
    /// <summary>
    /// Interaction logic for SearchForCustomerWindow.xaml
    /// </summary>
    public partial class SearchForCustomerWindow : Window
    {
        private readonly VetClinicService vetClinicService;

        public SearchForCustomerWindow(VetClinicService vetClinicService)
        {
            InitializeComponent();
            this.vetClinicService = vetClinicService;
        }

        public Customer? Customer { get; set; }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnSearchChanged(object sender, TextChangedEventArgs e)
        {
            searchResults.Items.Clear();
            var searchText = searchName.Text;
            if (searchText.Length < 3) return;

            // Instead of dataStore.FindCustomers, call:
            var customers = vetClinicService.FindCustomers(searchText);

            foreach (var customer in customers
                .OrderBy(c => c.Surname)
                .ThenBy(c => c.FirstName))
            {
                searchResults.Items.Add(new ListBoxItem { Content = customer });
            }
        }

        private void OnSelect(object sender, RoutedEventArgs e)
        {
            if (searchResults.SelectedItem == null) return;
            DialogResult = true;
            Customer = ((ListBoxItem)searchResults.SelectedItem).Content as Customer;
            Close();
        }
    }
}
