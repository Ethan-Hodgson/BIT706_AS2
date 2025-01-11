using System.Windows;
using Assignment2.App.ViewModels;

namespace Assignment2.App.Views
{
    public partial class EditCustomerWindow : Window
    {
        public EditCustomerWindow(EditCustomerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Subscribe to the ViewModel's RequestClose event
            viewModel.RequestClose += OnRequestClose;
        }

        private void OnRequestClose()
        {
            // Close the window when the event is raised
            Close();
        }
    }
}
