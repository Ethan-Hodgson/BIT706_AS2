using System;
using System.Windows.Input;
using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Assignment2.App.ViewModels
{
    public class EditCustomerViewModel : ObservableObject
    {
        private readonly VetClinicService vetClinicService;
        private Customer? currentCustomer;

        public EditCustomerViewModel(VetClinicService service, int customerId)
        {
            vetClinicService = service;
            LoadCustomer(customerId);

            SaveCommand = new RelayCommand(SaveChanges, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private string? firstName;
        public string? FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        private string? surname;
        public string? Surname
        {
            get => surname;
            set => SetProperty(ref surname, value);
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

        // Event to notify the view to close
        public event Action? RequestClose;

        private void LoadCustomer(int id)
        {
            currentCustomer = vetClinicService.GetCustomerById(id);
            if (currentCustomer != null)
            {
                FirstName = currentCustomer.FirstName;
                Surname = currentCustomer.Surname;
                PhoneNumber = currentCustomer.PhoneNumber;
                Address = currentCustomer.Address;
            }
        }

        private bool CanSave() =>
            !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(Surname);

        private void SaveChanges()
        {
            if (currentCustomer != null)
            {
                currentCustomer.FirstName = FirstName;
                currentCustomer.Surname = Surname;
                currentCustomer.PhoneNumber = PhoneNumber;
                currentCustomer.Address = Address;

                vetClinicService.UpdateCustomer(currentCustomer);

                // Notify the view to close
                RequestClose?.Invoke();
            }
        }

        private void Cancel()
        {
            // Notify the view to close without saving
            RequestClose?.Invoke();
        }
    }
}
