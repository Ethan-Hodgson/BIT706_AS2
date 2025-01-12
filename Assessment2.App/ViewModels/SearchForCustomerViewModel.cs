using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Assignment2.App.ViewModels
{
    public class SearchForCustomerViewModel : ObservableObject
    {
        private readonly VetClinicService vetClinicService;

        public SearchForCustomerViewModel(VetClinicService service)
        {
            vetClinicService = service;

            Customers = new ObservableCollection<Customer>();
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand(Select, CanSelect);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Properties
        public ObservableCollection<Customer> Customers { get; }

        private Customer? selectedCustomer;
        public Customer? SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                SetProperty(ref selectedCustomer, value);
                (SelectCommand as RelayCommand)?.NotifyCanExecuteChanged(); // Update SelectCommand state
            }
        }

        private string? searchText;
        public string? SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                Search();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand SelectCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? RequestClose;

        private void Search()
        {
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText.Length >= 3)
            {
                Customers.Clear();
                var results = vetClinicService.FindCustomers(SearchText);
                foreach (var customer in results)
                {
                    Customers.Add(customer);
                }
            }
        }

        private void Select()
        {
            RequestClose?.Invoke(); // Notify to close the window
        }

        private bool CanSelect()
        {
            return SelectedCustomer != null;
        }

        private void Cancel()
        {
            RequestClose?.Invoke(); // Notify to close the window
        }
    }
}
