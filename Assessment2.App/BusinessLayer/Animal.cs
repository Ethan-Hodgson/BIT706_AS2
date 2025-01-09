using System.IO;

namespace Assignment2.App.BusinessLayer
{
    public class Animal
    {
        public string? Breed { get; set; }

        public int Id { get; set; }

        public string? Name { get; set; }

        public int OwnerId { get; set; }

        public string? Sex { get; set; }

        public string? Type { get; set; }

        public static Animal FromCsv(string line)
        {
            var parts = line.Split(',');
            var animal = new Animal
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Type = parts[2],
                Breed = parts[3],
                Sex = parts[4],
                OwnerId = int.Parse(parts[5]),
            };

            return animal;
        }

        public static void WriteHeaderToCsv(TextWriter writer)
        {
            writer.WriteLine("Id,Name,Type,Breed,Sex,OwnerId");
        }

        public bool CheckIfValid()
        {
            return !(string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(Type)
                || string.IsNullOrEmpty(Sex)
                || (OwnerId == 0));
        }

        public override string ToString()
        {
            return $"{Name} [{Type}]";
        }

        public void WriteToCsv(TextWriter writer)
        {
            writer.WriteLine(string.Join(',', new[]
            {
                Id.ToString(),
                Name ?? string.Empty,
                Type ?? string.Empty,
                Breed ?? string.Empty,
                Sex ?? string.Empty,
                OwnerId.ToString(),
            }));
        }
    }
}