using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Assignment2.App.BusinessLayer;
using Assignment2.App.Repositories;

namespace Assignment2.App.BusinessLayer
{
    public class CsvAnimalRepository : IAnimalRepository
    {
        private readonly string animalsFilePath;
        private readonly List<Animal> animals = new();
        private int lastAnimalId = 0;

        public CsvAnimalRepository(string animalsFilePath)
        {
            this.animalsFilePath = animalsFilePath;
            LoadAll();
        }

        public IEnumerable<Animal> GetAll() => animals;

        public IEnumerable<Animal> FindByOwner(int ownerId) =>
            animals.Where(a => a.OwnerId == ownerId);

        public Animal? GetById(int animalId) =>
            animals.FirstOrDefault(a => a.Id == animalId);

        public void Add(Animal animal)
        {
            animal.Id = ++lastAnimalId;
            animals.Add(animal);
            SaveAll();
        }

        public void Update(Animal updatedAnimal)
        {
            var existing = GetById(updatedAnimal.Id);
            if (existing == null) return;

            existing.Name = updatedAnimal.Name;
            existing.Type = updatedAnimal.Type;
            existing.Breed = updatedAnimal.Breed;
            existing.Sex = updatedAnimal.Sex;
            existing.OwnerId = updatedAnimal.OwnerId;

            SaveAll();
        }

        public void Delete(int animalId)
        {
            var existing = GetById(animalId);
            if (existing != null)
            {
                animals.Remove(existing);
                SaveAll();
                Debug.WriteLine($"Deleted animal with ID: {animalId}");
            }
        }

        private void LoadAll()
        {
            animals.Clear();
            if (!File.Exists(animalsFilePath))
            {
                Debug.WriteLine("Animal file does not exist. No animals loaded.");
                return;
            }

            using var reader = new StreamReader(animalsFilePath);
            reader.ReadLine(); // Skip header

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var animal = Animal.FromCsv(line);
                animals.Add(animal);
                Debug.WriteLine($"Loaded Animal: {animal.Name}, Type={animal.Type}, OwnerId={animal.OwnerId}");
            }

            lastAnimalId = animals.Any() ? animals.Max(a => a.Id) : 0;
            Debug.WriteLine($"Total animals loaded: {animals.Count}");
        }


        private void SaveAll()
        {
            using var writer = new StreamWriter(animalsFilePath);
            Animal.WriteHeaderToCsv(writer);
            foreach (var animal in animals)
            {
                animal.WriteToCsv(writer);
            }
        }
    }
}
