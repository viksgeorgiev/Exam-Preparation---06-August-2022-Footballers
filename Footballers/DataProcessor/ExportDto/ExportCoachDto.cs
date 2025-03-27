using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlElement(nameof(CoachName))]
        public string CoachName { get; set; } = null!;

        [XmlAttribute(nameof(FootballersCount))]
        public int FootballersCount { get; set; }

        [XmlArray(nameof(Footballers))]
        public ExportFootballersDto[] Footballers { get; set; } = Array.Empty<ExportFootballersDto>();
    }
}
