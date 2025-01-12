using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Assignment2.App.ViewModels
{
    public class EditAnimalViewModel : ObservableObject
    {
        private readonly VetClinicService vetClinicService;
        private readonly Animal currentAnimal;

        public EditAnimalViewModel(VetClinicService service, Animal animal)
        {
            vetClinicService = service;
            currentAnimal = animal;

            // Initialize properties with existing animal data
            Name = currentAnimal.Name;
            Type = currentAnimal.Type;
            Breed = currentAnimal.Breed;
            Sex = currentAnimal.Sex;
            OwnerId = currentAnimal.OwnerId;

            // Populate lists
            Customers = new List<Customer>(vetClinicService.GetAllCustomers());
            AnimalTypes = new List<string> { "Dog", "Cat", "Bird", "Rabbit", "Reptile", "Fish", "Cow", "Sheep", "Goat", "Other" };
            AnimalSexes = new List<string> { "Male", "Female" };

            // Commands
            SaveCommand = new RelayCommand(SaveAnimal, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            DeleteCommand = new RelayCommand(DeleteAnimal);
        }

        // Event to notify the window to close
        public event Action<bool>? RequestClose; // true = refresh search window, false = just close

        // Properties
        private string? name;
        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string? type;
        public string? Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        private string? breed;
        public string? Breed
        {
            get => breed;
            set => SetProperty(ref breed, value);
        }

        private string? sex;
        public string? Sex
        {
            get => sex;
            set => SetProperty(ref sex, value);
        }

        private int ownerId;
        public int OwnerId
        {
            get => ownerId;
            set => SetProperty(ref ownerId, value);
        }

        public List<Customer> Customers { get; }
        public List<string> AnimalTypes { get; }
        public List<string> AnimalSexes { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteCommand { get; }

        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Type) &&
            !string.IsNullOrWhiteSpace(Sex) &&
            OwnerId > 0;

        private void SaveAnimal()
        {
            currentAnimal.Name = Name!;
            currentAnimal.Type = Type!;
            currentAnimal.Breed = Breed!;
            currentAnimal.Sex = Sex!;
            currentAnimal.OwnerId = OwnerId;

            vetClinicService.UpdateAnimal(currentAnimal);

            RequestClose?.Invoke(false); // Close without reopening the search window
        }

        private void DeleteAnimal()
        {
            // Prompt confirmation
            var result = MessageBox.Show(
                "Are you sure you want to delete this animal?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                // Perform delete
                vetClinicService.DeleteAnimal(currentAnimal.Id);

                // Notify the main window to refresh the search window
                RequestClose?.Invoke(true); // true = refresh and return to search
            }
        }

        private void Cancel()
        {
            RequestClose?.Invoke(false); // Notify to just close without reopening the search window
        }
    }
}
