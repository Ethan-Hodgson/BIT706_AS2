using Assignment2.App.ViewModels;
using System.Windows;

namespace Assignment2.App.Views
{
    public partial class SearchForCustomerWindow : Window
    {
        public SearchForCustomerWindow(SearchForCustomerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose += OnRequestClose;
        }

        private void OnRequestClose()
        {
            DialogResult = true;
            Close();
        }
    }
}
