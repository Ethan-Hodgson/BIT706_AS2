using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Assignment2.App.ViewModels
{
    public class SearchForAnimalViewModel : ObservableObject
    {
        private readonly VetClinicService vetClinicService;

        public SearchForAnimalViewModel(VetClinicService service)
        {
            vetClinicService = service;
            Animals = new ObservableCollection<Animal>();
            SelectCommand = new RelayCommand(Select, () => SelectedAnimal != null);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Observable collection of animals
        public ObservableCollection<Animal> Animals { get; }

        // Selected animal property
        private Animal? selectedAnimal;
        public Animal? SelectedAnimal
        {
            get => selectedAnimal;
            set
            {
                SetProperty(ref selectedAnimal, value);
                (SelectCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        // Commands for Select and Cancel
        public ICommand SelectCommand { get; }
        public ICommand CancelCommand { get; }

        // Event to notify the view to close
        public event Action? RequestClose;

        // Load animals by owner ID
        public void LoadAnimalsByOwner(int ownerId)
        {
            Animals.Clear();
            var animals = vetClinicService.GetAnimalsByOwner(ownerId);
            foreach (var animal in animals)
            {
                Animals.Add(animal);
                Debug.WriteLine($"Added to ObservableCollection: {animal.Name}, OwnerId={animal.OwnerId}");
            }
            Debug.WriteLine($"Total animals loaded into ObservableCollection: {Animals.Count}");
        }

        // Load all animals
        public void LoadAnimals()
        {
            Animals.Clear(); // Clear existing items
            var animals = vetClinicService.GetAllAnimals(); // Fetch all animals
            foreach (var animal in animals)
            {
                Animals.Add(animal); // Add them to the observable collection
                Debug.WriteLine($"Loaded Animal: {animal.Name}, OwnerId={animal.OwnerId}");
            }
            Debug.WriteLine($"Total animals loaded: {Animals.Count}");
        }


        private void Select()
        {
            RequestClose?.Invoke();
        }

        private void Cancel()
        {
            RequestClose?.Invoke();
        }
    }
}
