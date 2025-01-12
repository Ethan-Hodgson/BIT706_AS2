using System.IO;

namespace Assignment2.App.BusinessLayer
{
    public class Animal
    {
        public string? Breed { get; set; } = "Unknown";
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string? Sex { get; set; } = "Unknown";
        public string? Type { get; set; } = string.Empty;

        public static Animal FromCsv(string line)
        {
            var parts = line.Split(',');
            return new Animal
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                Type = parts[2],
                Breed = parts[3],
                Sex = parts[4],
                OwnerId = int.Parse(parts[5])
            };
        }

        public static void WriteHeaderToCsv(TextWriter writer)
        {
            writer.WriteLine("Id,Name,Type,Breed,Sex,OwnerId");
        }

        public void WriteToCsv(TextWriter writer)
        {
            writer.WriteLine($"{Id},{Name},{Type},{Breed},{Sex},{OwnerId}");
        }
    }
}
