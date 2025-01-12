using Assignment2.App.BusinessLayer;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;

public class AddAnimalViewModel : ObservableObject
{
    private readonly VetClinicService vetClinicService;
    private readonly Window currentWindow;

    public AddAnimalViewModel(VetClinicService service, Window window)
    {
        vetClinicService = service;
        currentWindow = window;

        SaveCommand = new RelayCommand(SaveAnimal, CanSave);
        CancelCommand = new RelayCommand(Cancel);

        // Populate Customers, Animal Types, and Animal Sexes
        Customers = new List<Customer>(vetClinicService.GetAllCustomers());
        AnimalTypes = new List<string> { "Dog", "Cat", "Bird", "Rabbit", "Reptile", "Fish", "Cow", "Sheep", "Goat", "Other" };
        AnimalSexes = new List<string> { "Male", "Female" };
    }




    // Properties
    private string? name;
    public string? Name
    {
        get => name;
        set
        {
            SetProperty(ref name, value);
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    private string? type;
    public string? Type
    {
        get => type;
        set
        {
            SetProperty(ref type, value);
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    private string? breed;
    public string? Breed
    {
        get => breed;
        set
        {
            SetProperty(ref breed, value);
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    private string? sex;
    public string? Sex
    {
        get => sex;
        set
        {
            SetProperty(ref sex, value);
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    private int ownerId;
    public int OwnerId
    {
        get => ownerId;
        set
        {
            SetProperty(ref ownerId, value);
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
    }

    public List<Customer> Customers { get; }
    public List<string> AnimalTypes { get; }
    public List<string> AnimalSexes { get; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public event Action? RequestClose;

    private bool CanSave() =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(Type) &&
        !string.IsNullOrWhiteSpace(Sex) &&
        OwnerId > 0;

    private void SaveAnimal()
    {
        Debug.WriteLine("SaveAnimal called");
        var animal = new Animal
        {
            Name = Name!,
            Type = Type!,
            Breed = Breed!,
            Sex = Sex!,
            OwnerId = OwnerId
        };

        vetClinicService.CreateAnimal(animal);
        Debug.WriteLine($"Animal saved: {animal.Name}, Type={animal.Type}, OwnerId={animal.OwnerId}");
        RequestClose?.Invoke();
    }

    private void Cancel()
    {
        RequestClose?.Invoke();
    }
}
