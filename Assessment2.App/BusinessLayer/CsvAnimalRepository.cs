using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assignment2.App.BusinessLayer;
using Assignment2.App.Repositories;

namespace Assignment2.App.BusinessLayer
{
    public class CsvAnimalRepository : IAnimalRepository
    {
        private readonly string animalsFilePath;

        // We'll keep an in-memory list of animals, loaded from CSV
        private List<Animal> animals = new List<Animal>();
        private int lastAnimalId = 0;

        public CsvAnimalRepository(string animalsFilePath)
        {
            this.animalsFilePath = animalsFilePath;
            LoadAll();  // Load from CSV on construction
        }

        public IEnumerable<Animal> GetAll()
        {
            // Return a copy or read-only reference
            return animals;
        }

        public IEnumerable<Animal> FindByOwner(int ownerId)
        {
            // Return animals matching the given ownerId
            return animals.Where(a => a.OwnerId == ownerId);
        }

        public Animal? GetById(int animalId)
        {
            return animals.FirstOrDefault(a => a.Id == animalId);
        }

        public void Add(Animal animal)
        {
            // Assign an ID by incrementing
            animal.Id = ++lastAnimalId;

            animals.Add(animal);
            SaveAll();
        }

        public void Update(Animal updatedAnimal)
        {
            // Find the existing one
            var existing = GetById(updatedAnimal.Id);
            if (existing == null)
                return; // or throw an exception

            // Update fields
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
            }
        }

        // ------------------------------------------
        // Private helpers to load/save the CSV file
        // ------------------------------------------

        private void LoadAll()
        {
            animals.Clear();
            if (!File.Exists(animalsFilePath))
            {
                // If file doesn't exist, no animals to load
                lastAnimalId = 0;
                return;
            }

            using var stream = File.OpenRead(animalsFilePath);
            using var reader = new StreamReader(stream);
            // First line is header - read & discard
            var headerLine = reader.ReadLine();

            var line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                var animal = Animal.FromCsv(line);
                animals.Add(animal);
                line = reader.ReadLine();
            }

            // Set our lastAnimalId to the max found in file
            lastAnimalId = animals.Any() ? animals.Max(a => a.Id) : 0;
        }

        private void SaveAll()
        {
            using var stream = File.Create(animalsFilePath);
            using var writer = new StreamWriter(stream);
            // Write header
            Animal.WriteHeaderToCsv(writer);

            // Write each animal line
            foreach (var animal in animals)
            {
                animal.WriteToCsv(writer);
            }
        }
    }
}
