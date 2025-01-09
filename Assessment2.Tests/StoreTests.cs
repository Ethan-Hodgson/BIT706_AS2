using Assignment2.App.BusinessLayer;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Assignment2.Tests
{
    public class StoreTests
    {
        // 1) AddAnimal
        [Fact]
        public void AddAnimal_IncrementsIdAndReturnsNewAnimal()
        {
            // Check that each new animal ID is incremented.
            var store = new Store();
            var animal1 = store.AddAnimal();
            var animal2 = store.AddAnimal();

            // IDs should be 1 and 2 respectively
            Assert.Equal(1, animal1.Id);
            Assert.Equal(2, animal2.Id);
        }

        // 2) AddCustomer
        [Fact]
        public void AddCustomer_IncrementsIdAndReturnsNewCustomer()
        {
            var store = new Store();
            var cust1 = store.AddCustomer();
            var cust2 = store.AddCustomer();

            Assert.Equal(1, cust1.Id);
            Assert.Equal(2, cust2.Id);
        }

        // 3) FindAnimals
        [Fact]
        public void FindAnimals_ReturnsAnimalsForOwnerId()
        {
            var store = new Store();

            // Create animals with different owner IDs
            var a1 = store.AddAnimal();
            a1.OwnerId = 1;
            store.Animals.Add(a1);

            var a2 = store.AddAnimal();
            a2.OwnerId = 2;
            store.Animals.Add(a2);

            var a3 = store.AddAnimal();
            a3.OwnerId = 1;
            store.Animals.Add(a3);

            // Now search for ownerId=1
            var results = store.FindAnimals(1).ToList();

            // We should get a1, a3
            Assert.Equal(2, results.Count);
            Assert.All(results, a => Assert.Equal(1, a.OwnerId));
        }

        // 4) FindCustomers
        [Fact]
        public void FindCustomers_ReturnsMatchesCaseInsensitive()
        {
            var store = new Store();

            var c1 = store.AddCustomer();
            c1.FirstName = "John";
            c1.Surname = "Doe";
            store.Customers.Add(c1);

            var c2 = store.AddCustomer();
            c2.FirstName = "JoHnNy";
            c2.Surname = "Appleseed";
            store.Customers.Add(c2);

            var c3 = store.AddCustomer();
            c3.FirstName = "Alice";
            c3.Surname = "Johnson";
            store.Customers.Add(c3);

            // Searching for "john" should match "John" and "JoHnNy" and "Johnson"
            var results = store.FindCustomers("john").ToList();

            // We expect c1, c2, c3 (all have "john" substring ignoring case)
            Assert.Equal(3, results.Count);
        }

        // 5) Save & Load (This indirectly tests LoadAnimals, LoadCustomers, SaveAnimals, SaveCustomers)
        [Fact]
        public void SaveAndLoad_SavesAllDataToDiskAndReloads()
        {
            var testFolder = Path.Combine(Path.GetTempPath(), "StoreTests_" + Guid.NewGuid());
            Directory.CreateDirectory(testFolder);

            try
            {
                var store = new Store();

                var cust = store.AddCustomer();
                cust.FirstName = "Alice";
                cust.Surname = "Wonderland";
                cust.PhoneNumber = "000-1111";
                cust.Address = "123 Some St\nLine2";
                store.Customers.Add(cust);

                var animal = store.AddAnimal();
                animal.Name = "Cheshire Cat";
                animal.Type = "Cat";
                animal.Breed = "Tabby";
                animal.Sex = "Male";
                animal.OwnerId = cust.Id;
                store.Animals.Add(animal);

                // Save to disk
                store.Save(testFolder);

                // Create a new store and load from disk
                var store2 = new Store();
                store2.Load(testFolder);

                // Check reloaded data
                Assert.Single(store2.Customers);
                Assert.Single(store2.Animals);

                var reloadedCustomer = store2.Customers.First();
                Assert.Equal("Alice", reloadedCustomer.FirstName);
                Assert.Equal("Wonderland", reloadedCustomer.Surname);
                Assert.Equal("000-1111", reloadedCustomer.PhoneNumber);
                Assert.Equal("123 Some St\r\nLine2", reloadedCustomer.Address);

                var reloadedAnimal = store2.Animals.First();
                Assert.Equal("Cheshire Cat", reloadedAnimal.Name);
                Assert.Equal("Cat", reloadedAnimal.Type);
                Assert.Equal("Tabby", reloadedAnimal.Breed);
                Assert.Equal("Male", reloadedAnimal.Sex);
                Assert.Equal(1, reloadedAnimal.OwnerId);
            }
            finally
            {
                // Clean up
                Directory.Delete(testFolder, true);
            }
        }
    }
}
