using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assignment2.App.BusinessLayer;
using Assignment2.App.Repositories;

namespace Assignment2.App.BusinessLayer
{
    public class CsvCustomerRepository : ICustomerRepository
    {
        private readonly string customersFilePath;
        private List<Customer> customers = new();
        private int lastCustomerId = 0;

        public CsvCustomerRepository(string customersFilePath)
        {
            this.customersFilePath = customersFilePath;

            // Ensure directory exists
            var directory = Path.GetDirectoryName(customersFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            LoadAll();
        }

        public IEnumerable<Customer> GetAll() => customers;

        public IEnumerable<Customer> FindByName(string name) =>
            customers.Where(c =>
                (c.FirstName?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (c.Surname?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false));

        public Customer? GetById(int customerId) =>
            customers.FirstOrDefault(c => c.Id == customerId);

        public void Add(Customer customer)
        {
            customer.Id = ++lastCustomerId;
            customers.Add(customer);
            SaveAll();
        }

        public void Update(Customer updatedCustomer)
        {
            var existing = GetById(updatedCustomer.Id);
            if (existing == null) return;

            existing.FirstName = updatedCustomer.FirstName;
            existing.Surname = updatedCustomer.Surname;
            existing.PhoneNumber = updatedCustomer.PhoneNumber;
            existing.Address = updatedCustomer.Address;

            SaveAll();
        }

        public void Delete(int customerId)
        {
            var existing = GetById(customerId);
            if (existing != null)
            {
                customers.Remove(existing);
                SaveAll();
            }
        }

        private void LoadAll()
        {
            customers.Clear();
            if (!File.Exists(customersFilePath)) return;

            using var reader = new StreamReader(customersFilePath);
            reader.ReadLine(); // Skip header
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                customers.Add(Customer.FromCsv(line));
            }

            lastCustomerId = customers.Any() ? customers.Max(c => c.Id) : 0;
        }

        private void SaveAll()
        {
            using var writer = new StreamWriter(customersFilePath);
            Customer.WriteHeaderToCsv(writer);
            foreach (var customer in customers)
            {
                customer.WriteToCsv(writer);
            }
        }
    }
}
