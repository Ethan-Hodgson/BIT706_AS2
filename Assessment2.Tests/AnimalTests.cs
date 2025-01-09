using Assignment2.App.BusinessLayer;
using System.IO;
using Xunit;

namespace Assignment2.Tests
{
    public class AnimalTests
    {
        // 1) CheckIfValid — covered partially by existing tests
        [Theory]
        [InlineData("Bob", "Dog", "Male", 1, true)]
        [InlineData(null, "Dog", "Male", 1, false)]
        [InlineData("Bob", null, "Male", 1, false)]
        [InlineData("Bob", "Dog", null, 1, false)]
        [InlineData("Bob", "Dog", "Male", 0, false)]
        public void CheckIfValid_ReturnsExpectedResult(string? name, string? type, string? sex, int ownerId, bool expected)
        {
            // This test checks that CheckIfValid identifies when properties are null/empty or missing.
            var animal = new Animal { Name = name, Type = type, Sex = sex, OwnerId = ownerId };

            var result = animal.CheckIfValid();

            Assert.Equal(expected, result);
        }

        // 2) FromCsv
        [Fact]
        public void FromCsv_ParsesCsvLineCorrectly()
        {
            // This test ensures fromCsv can handle a typical CSV line for animals.
            // Example line: "1,Fluffy,Cat,Persian,Female,99"
            string csvLine = "1,Fluffy,Cat,Persian,Female,99";

            var result = Animal.FromCsv(csvLine);

            Assert.Equal(1, result.Id);
            Assert.Equal("Fluffy", result.Name);
            Assert.Equal("Cat", result.Type);
            Assert.Equal("Persian", result.Breed);
            Assert.Equal("Female", result.Sex);
            Assert.Equal(99, result.OwnerId);
        }

        // 3) WriteToCsv
        [Fact]
        public void WriteToCsv_WritesCorrectFormat()
        {
            // This test confirms that WriteToCsv writes the correct CSV format
            // which can be later read back by FromCsv if needed.
            var animal = new Animal
            {
                Id = 2,
                Name = "Buddy",
                Type = "Dog",
                Breed = "Labrador",
                Sex = "Male",
                OwnerId = 10
            };

            using var writer = new StringWriter();
            animal.WriteToCsv(writer);
            var csvOutput = writer.ToString().Trim(); // e.g. "2,Buddy,Dog,Labrador,Male,10"

            Assert.Equal("2,Buddy,Dog,Labrador,Male,10", csvOutput);
        }

        // 4) ToString
        [Fact]
        public void ToString_ReturnsNameAndType()
        {
            // This test ensures that the overridden ToString() method produces "Name [Type]".
            var animal = new Animal { Name = "Bobby", Type = "Cat" };

            var output = animal.ToString();

            Assert.Equal("Bobby [Cat]", output);
        }
    }
}
