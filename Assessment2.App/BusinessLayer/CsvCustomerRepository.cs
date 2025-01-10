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

        private List<Customer> customers = new List<Customer>();
        private int lastCustomerId = 0;

        public CsvCustomerRepository(string customersFilePath)
        {
            this.customersFilePath = customersFilePath;
            LoadAll();
        }

        public IEnumerable<Customer> GetAll()
        {
            return customers;
        }

        public IEnumerable<Customer> FindByName(string name)
        {
            // Example: case-insensitive substring match
            return customers.Where(c =>
                (c.FirstName != null && c.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase)) ||
                (c.Surname != null && c.Surname.Contains(name, StringComparison.OrdinalIgnoreCase)));
        }

        public Customer? GetById(int customerId)
        {
            return customers.FirstOrDefault(c => c.Id == customerId);
        }

        public void Add(Customer customer)
        {
            customer.Id = ++lastCustomerId;
            customers.Add(customer);
            SaveAll();
        }

        public void Update(Customer updatedCustomer)
        {
            var existing = GetById(updatedCustomer.Id);
            if (existing == null)
                return; // or throw

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

        // --------------------------
        // Private Helpers
        // --------------------------
        private void LoadAll()
        {
            customers.Clear();
            if (!File.Exists(customersFilePath))
            {
                lastCustomerId = 0;
                return;
            }

            using var stream = File.OpenRead(customersFilePath);
            using var reader = new StreamReader(stream);
            // first line is header
            var headerLine = reader.ReadLine();

            var line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                var cust = Customer.FromCsv(line);
                customers.Add(cust);
                line = reader.ReadLine();
            }

            lastCustomerId = customers.Any() ? customers.Max(c => c.Id) : 0;
        }

        private void SaveAll()
        {
            using var stream = File.Create(customersFilePath);
            using var writer = new StreamWriter(stream);
            Customer.WriteHeaderToCsv(writer);

            foreach (var cust in customers)
            {
                cust.WriteToCsv(writer);
            }
        }
    }
}
