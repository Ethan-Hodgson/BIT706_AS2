using Assignment2.App.ViewModels;
using System.Windows;

namespace Assignment2.App.Views
{
    public partial class EditAnimalWindow : Window
    {
        public EditAnimalWindow(EditAnimalViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose += OnRequestClose;
        }

        private void OnRequestClose(bool navigateToSearch)
        {
            DialogResult = navigateToSearch; // Return true if navigating to the search window
            Close();
        }
    }
}
