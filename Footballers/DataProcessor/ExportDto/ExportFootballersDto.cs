using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportFootballersDto
    {
        [XmlElement(nameof(Name))]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Position))]
        public string Position { get; set; } = null!;
    }
}