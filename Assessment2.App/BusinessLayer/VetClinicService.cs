using System.Collections.Generic;
using Assignment2.App.Repositories;

namespace Assignment2.App.BusinessLayer
{
    public class VetClinicService
    {
        private readonly IAnimalRepository animalRepo;
        private readonly ICustomerRepository customerRepo;

        public VetClinicService(
            IAnimalRepository animalRepository,
            ICustomerRepository customerRepository)
        {
            this.animalRepo = animalRepository;
            this.customerRepo = customerRepository;
        }

        // -----------------------
        // ANIMAL-RELATED METHODS
        // -----------------------

        public Animal CreateAnimal(Animal animal)
        {
            // If needed, do validation: if (!animal.CheckIfValid()) throw ...
            animalRepo.Add(animal);
            return animal;
        }

        public void UpdateAnimal(Animal animal)
        {
            animalRepo.Update(animal);
        }

        public void DeleteAnimal(int animalId)
        {
            animalRepo.Delete(animalId);
        }

        public IEnumerable<Animal> GetAnimalsByOwner(int ownerId)
        {
            // If you want "all animals" when ownerId=0, you can do:
            // if (ownerId == 0) return animalRepo.GetAll();
            // else return animalRepo.FindByOwner(ownerId);

            return animalRepo.FindByOwner(ownerId);
        }

        // -------------------------
        // CUSTOMER-RELATED METHODS
        // -------------------------

        public Customer CreateCustomer(Customer customer)
        {
            // if (!customer.CheckIfValid()) throw ...
            customerRepo.Add(customer);
            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            customerRepo.Update(customer);
        }

        public void DeleteCustomer(int customerId)
        {
            customerRepo.Delete(customerId);
        }

        public IEnumerable<Customer> FindCustomers(string name)
        {
            // name could be partial or case-insensitive
            return customerRepo.FindByName(name);
        }
    }
}
