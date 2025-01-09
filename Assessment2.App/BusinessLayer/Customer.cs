using System;
using System.IO;

namespace Assignment2.App.BusinessLayer
{
    public class Customer
    {
        public string? Address { get; set; }

        public string? FirstName { get; set; }

        public int Id { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Surname { get; set; }

        public static Customer FromCsv(string line)
        {
            var parts = line.Split(',');
            var customer = new Customer
            {
                Id = int.Parse(parts[0]),
                FirstName = parts[1],
                Surname = parts[2],
                PhoneNumber = parts[3],
                Address = parts[4][1..^1].Replace("\\n", Environment.NewLine),
            };

            return customer;
        }

        public static void WriteHeaderToCsv(TextWriter writer)
        {
            writer.WriteLine("Id,First Name,Surname,Phone,Address");
        }

        public bool CheckIfValid()
        {
            return !(string.IsNullOrEmpty(FirstName)
                || string.IsNullOrEmpty(PhoneNumber)
                || string.IsNullOrEmpty(Surname));
        }

        public override string ToString()
        {
            return $"{Surname}, {FirstName}".Trim(); ;
        }

        public void WriteToCsv(TextWriter writer)
        {
            var address = Address ?? string.Empty;

            // Remove newlines so they don't screw up the reading later
            address = address.Replace("\n", "\\n");
            address = address.Replace("\r", "");

            writer.WriteLine(string.Join(',', new[]
            {
                Id.ToString(),
                FirstName ?? string.Empty,
                Surname ?? string.Empty,
                PhoneNumber ?? string.Empty,
                "\"" + address + "\"",
            }));
        }
    }
}