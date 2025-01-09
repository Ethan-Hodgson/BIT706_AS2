using Assignment2.App.BusinessLayer;
using System.IO;
using Xunit;

namespace Assignment2.Tests
{
    public class CustomerTests
    {
        // 1) CheckIfValid
        [Theory]
        [InlineData("John", "Doe", "123-4567", true)]
        [InlineData(null, "Doe", "123-4567", false)]
        [InlineData("John", null, "123-4567", false)]
        [InlineData("John", "Doe", null, false)]
        public void CheckIfValid_ReturnsExpectedResult(string? firstName, string? surname, string? phoneNumber, bool expected)
        {
            // Tests that CheckIfValid identifies missing or empty fields for Customer.
            var customer = new Customer
            {
                FirstName = firstName,
                Surname = surname,
                PhoneNumber = phoneNumber
            };

            var result = customer.CheckIfValid();

            Assert.Equal(expected, result);
        }

        // 2) FromCsv
        [Fact]
        public void FromCsv_ParsesCsvLineCorrectly()
        {
            // Example line: "1,John,Doe,123-4567,\"123 Some St\nLine2\""
            var csvLine = "1,John,Doe,123-4567,\"123 Some St\nLine2\"";

            var result = Customer.FromCsv(csvLine);

            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.Surname);
            Assert.Equal("123-4567", result.PhoneNumber);
            Assert.Equal("123 Some St\nLine2", result.Address);
        }

        // 3) WriteToCsv
        [Fact]
        public void WriteToCsv_WritesCorrectFormat()
        {
            // Confirms the CSV line is properly escaped, especially for addresses containing new lines.
            var customer = new Customer
            {
                Id = 2,
                FirstName = "Jane",
                Surname = "Smith",
                PhoneNumber = "987-6543",
                Address = "45 Road\nAnother Line"
            };

            using var writer = new StringWriter();
            customer.WriteToCsv(writer);
            var csvOutput = writer.ToString().Trim();

            // Expected e.g.:
            // 2,Jane,Smith,987-6543,"45 Road\nAnother Line"
            Assert.Contains("2,Jane,Smith,987-6543", csvOutput);
            Assert.Contains("\"45 Road\\nAnother Line\"", csvOutput);
        }

        // 4) ToString
        [Fact]
        public void ToString_ReturnsSurnameCommaFirstName()
        {
            var customer = new Customer
            {
                FirstName = "John",
                Surname = "Doe"
            };

            var result = customer.ToString();

            Assert.Equal("Doe, John", result);
        }
    }
}
