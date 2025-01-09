using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Assignment2.App.BusinessLayer
{
    public class Store
    {
        private int lastAnimalId = 0;
        private int lastCustomerId = 0;

        public List<Animal> Animals { get; } = new();

        public List<Customer> Customers { get; } = new();

        public Animal AddAnimal()
        {
            var animal = new Animal
            {
                Id = ++lastAnimalId,
            };
            return animal;
        }

        public Customer AddCustomer()
        {
            var customer = new Customer
            {
                Id = ++lastCustomerId,
            };
            return customer;
        }

        public IEnumerable<Animal> FindAnimals(int ownerId)
        {
            var animals = this.Animals
                .Where(a => a.OwnerId == ownerId);
            return animals;
        }

        public IEnumerable<Customer> FindCustomers(string name)
        {
            var customers = Customers
                .Where(c => c.FirstName?.Contains(name, System.StringComparison.InvariantCultureIgnoreCase) == true
                         || c.Surname?.Contains(name, System.StringComparison.InvariantCultureIgnoreCase) == true);
            return customers;
        }

        public void Load(string path)
        {
            var animalsPath = Path.Combine(path, "animals.csv");
            var customersPath = Path.Combine(path, "customers.csv");

            if (File.Exists(animalsPath)) LoadAnimals(animalsPath);
            if (File.Exists(customersPath)) LoadCustomers(customersPath);
        }

        public void Save(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var animalsPath = Path.Combine(path, "animals.csv");
            var customersPath = Path.Combine(path, "customers.csv");

            SaveAnimals(animalsPath);
            SaveCustomers(customersPath);
        }

        private void LoadAnimals(string path)
        {
            Animals.Clear();
            using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream);
            var line = reader.ReadLine();
            line = reader.ReadLine();   // First line is a header - need to skip it
            while (!string.IsNullOrEmpty(line))
            {
                Animals.Add(Animal.FromCsv(line));
                line = reader.ReadLine();
            }

            lastAnimalId = Animals.Any() ? Animals.Max(a => a.Id) : 0;
        }

        private void LoadCustomers(string path)
        {
            Customers.Clear();
            using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream);
            var line = reader.ReadLine();
            line = reader.ReadLine();   // First line is a header - need to skip it
            while (!string.IsNullOrEmpty(line))
            {
                Customers.Add(Customer.FromCsv(line));
                line = reader.ReadLine();
            }

            lastCustomerId = Animals.Any() ? Animals.Max(a => a.Id) : 0;
        }

        private void SaveAnimals(string path)
        {
            using var stream = File.Create(path);
            using var writer = new StreamWriter(stream);
            Animal.WriteHeaderToCsv(writer);
            foreach (var animal in Animals)
            {
                animal.WriteToCsv(writer);
            }
        }

        private void SaveCustomers(string path)
        {
            using var stream = File.Create(path);
            using var writer = new StreamWriter(stream);
            Customer.WriteHeaderToCsv(writer);
            foreach (var customer in Customers)
            {
                customer.WriteToCsv(writer);
            }
        }
    }
}