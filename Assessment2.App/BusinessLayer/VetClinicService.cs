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
            animalRepo = animalRepository;
            customerRepo = customerRepository;
        }

        // ANIMAL METHODS
        public Animal CreateAnimal(Animal animal)
        {
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
            return ownerId == 0 ? animalRepo.GetAll() : animalRepo.FindByOwner(ownerId);
        }

        // CUSTOMER METHODS
        public Customer CreateCustomer(Customer customer)
        {
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
            return customerRepo.FindByName(name);
        }

        public Customer? GetCustomerById(int customerId)
        {
            return customerRepo.GetById(customerId);
        }
    }
}
