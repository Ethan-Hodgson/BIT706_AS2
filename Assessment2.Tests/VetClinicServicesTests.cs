using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// File: VetClinicServiceTests.cs

using Assignment2.App.BusinessLayer;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Assignment2.Tests
{
    public class VetClinicServiceTests
    {
        private readonly string testFolder;
        private readonly VetClinicService service;

        public VetClinicServiceTests()
        {
            // 1) Set up a temporary folder for CSV testing
            testFolder = Path.Combine(Path.GetTempPath(), "VetClinicServiceTest_" + Guid.NewGuid());
            Directory.CreateDirectory(testFolder);

            // 2) Create CSV-based repositories, pointing to that folder
            var animalsPath = Path.Combine(testFolder, "animals.csv");
            var customersPath = Path.Combine(testFolder, "customers.csv");

            var animalRepo = new CsvAnimalRepository(animalsPath);
            var customerRepo = new CsvCustomerRepository(customersPath);

            // 3) Create the service with those repositories
            service = new VetClinicService(animalRepo, customerRepo);
        }

        [Fact]
        public void CreateAnimal_ShouldAssignIdAndPersistAnimal()
        {
            // This replaces Store.AddAnimal logic + verifying the new Animal is persisted
            var animal = new Animal
            {
                Name = "Bobby",
                Type = "Dog",
                Breed = "Labrador",
                Sex = "Male"
                // ID might be assigned by the repository or the service
            };

            var result = service.CreateAnimal(animal);

            // Make sure the ID was assigned
            // Where you do auto-increment is up to you (repository or service).
            Assert.True(result.Id > 0);

            // Optionally verify we can retrieve it from the repository
            var allAnimals = service.GetAnimalsByOwner(0).ToList();
            // or an equivalent method if you're not filtering by owner
            Assert.Contains(allAnimals, a => a.Name == "Bobby");
        }

        [Fact]
        public void CreateCustomer_ShouldAssignIdAndPersistCustomer()
        {
            var customer = new Customer
            {
                FirstName = "Alice",
                Surname = "Wonderland",
                PhoneNumber = "000-1111"
            };

            var result = service.CreateCustomer(customer);

            Assert.True(result.Id > 0);

            // Possibly retrieve to verify
            var matchingCustomers = service.FindCustomers("Alice").ToList();
            Assert.Single(matchingCustomers);
            Assert.Equal("Alice", matchingCustomers[0].FirstName);
        }

        [Fact]
        public void GetAnimalsByOwner_ShouldReturnCorrectAnimals()
        {
            // Similar to Store.FindAnimals test but now we call VetClinicService
            var owner = service.CreateCustomer(new Customer
            {
                FirstName = "John",
                Surname = "Doe",
                PhoneNumber = "123-4567"
            });

            // Create a couple animals for that owner
            var a1 = service.CreateAnimal(new Animal { Name = "Fluffy", OwnerId = owner.Id });
            var a2 = service.CreateAnimal(new Animal { Name = "Rex", OwnerId = owner.Id });

            // Also create an animal for a different owner
            var otherOwner = service.CreateCustomer(new Customer
            {
                FirstName = "Jane",
                Surname = "Smith",
                PhoneNumber = "987-6543"
            });
            var a3 = service.CreateAnimal(new Animal { Name = "NotJohns", OwnerId = otherOwner.Id });

            // Now get the animals for John
            var results = service.GetAnimalsByOwner(owner.Id).ToList();

            Assert.Equal(2, results.Count);
            Assert.Contains(results, a => a.Name == "Fluffy");
            Assert.Contains(results, a => a.Name == "Rex");
        }

        [Fact]
        public void FindCustomers_ShouldReturnMatches()
        {
            // Replaces Store.FindCustomers test
            service.CreateCustomer(new Customer { FirstName = "Johnny", Surname = "Appleseed", PhoneNumber = "111-1111" });
            service.CreateCustomer(new Customer { FirstName = "Alice", Surname = "Johnson", PhoneNumber = "222-2222" });
            service.CreateCustomer(new Customer { FirstName = "John", Surname = "Doe", PhoneNumber = "333-3333" });

            // Searching for "john" should return 3 matches if ignoring case
            var results = service.FindCustomers("john").ToList();
            Assert.Equal(3, results.Count);
        }

        [Fact]
        public void SaveAndLoad_ShouldPersistAllData()
        {
            // If your VetClinicService or repositories have "Save" / "Load" logic
            // replicate the end-to-end test from your old Store.

            // 1) Create some data
            var cust = service.CreateCustomer(new Customer
            {
                FirstName = "Bob",
                Surname = "Marley",
                PhoneNumber = "123"
            });

            var anim = service.CreateAnimal(new Animal
            {
                Name = "Ziggy",
                Type = "Cat",
                OwnerId = cust.Id
            });

            // 2) If your CSV repositories automatically save on Add/Update,
            //    or if there's an explicit Save() method, call it here.
            //    e.g. service.Save() if you created such a method
            //    If not, your CSV repo might be writing changes immediately.

            // 3) Create a new service with fresh repos to simulate a new session
            var newAnimalRepo = new CsvAnimalRepository(Path.Combine(testFolder, "animals.csv"));
            var newCustomerRepo = new CsvCustomerRepository(Path.Combine(testFolder, "customers.csv"));
            var newService = new VetClinicService(newAnimalRepo, newCustomerRepo);

            // 4) Retrieve data from newService
            var allCusts = newService.FindCustomers("Bob").ToList();
            var allAnims = newService.GetAnimalsByOwner(cust.Id).ToList();

            Assert.Single(allCusts);
            Assert.Single(allAnims);

            var reloadedCust = allCusts.First();
            var reloadedAnim = allAnims.First();

            Assert.Equal("Bob", reloadedCust.FirstName);
            Assert.Equal("Marley", reloadedCust.Surname);
            Assert.Equal("Ziggy", reloadedAnim.Name);
            Assert.Equal("Cat", reloadedAnim.Type);
            Assert.Equal(cust.Id, reloadedAnim.OwnerId);
        }

        // OPTIONAL: Cleanup
        ~VetClinicServiceTests()
        {
            try
            {
                Directory.Delete(testFolder, true);
            }
            catch { /* ignore */ }
        }
    }
}
