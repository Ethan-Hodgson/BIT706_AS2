using System;
using System.Windows.Input;
using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Assignment2.App.ViewModels
{
    public class AddCustomerViewModel : BaseViewModel
    {
        private readonly VetClinicService vetClinicService;

        // Event to notify when the window should close
        public event Action? RequestClose;

        public AddCustomerViewModel(VetClinicService service)
        {
            vetClinicService = service;

            SaveCommand = new RelayCommand(SaveNewCustomer, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private string? firstName;
        public string? FirstName
        {
            get => firstName;
            set
            {
                SetProperty(ref firstName, value);
                (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged(); // Notify CanExecuteChanged
            }
        }

        private string? surname;
        public string? Surname
        {
            get => surname;
            set
            {
                SetProperty(ref surname, value);
                (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged(); // Notify CanExecuteChanged
            }
        }

        private string? phoneNumber;
        public string? PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        private string? address;
        public string? Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private bool CanSave() =>
            !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Surname);

        private void SaveNewCustomer()
        {
            var newCustomer = new Customer
            {
                FirstName = FirstName,
                Surname = Surname,
                PhoneNumber = PhoneNumber,
                Address = Address
            };

            vetClinicService.CreateCustomer(newCustomer);

            // Notify the view to close
            RequestClose?.Invoke();
        }

        private void Cancel()
        {
            // Notify the view to close
            RequestClose?.Invoke();
        }
    }
}
