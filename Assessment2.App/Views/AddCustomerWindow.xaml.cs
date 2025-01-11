using System.Windows;
using Assignment2.App.ViewModels;

namespace Assignment2.App.Views
{
    public partial class AddCustomerWindow : Window
    {
        public AddCustomerWindow(AddCustomerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Subscribe to the RequestClose event
            viewModel.RequestClose += OnRequestClose;
        }

        private void OnRequestClose()
        {
            Close();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            if (DataContext is AddCustomerViewModel viewModel)
            {
                viewModel.RequestClose -= OnRequestClose; // Unsubscribe to avoid memory leaks
            }
            base.OnClosed(e);
        }
    }
}
