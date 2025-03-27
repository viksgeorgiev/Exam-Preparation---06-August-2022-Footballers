using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    [XmlType("Coach")]
    public class ImportCoachesDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(Nationality))]
        [Required]
        public string Nationality { get; set; } = null!;

        [XmlArray(nameof(Footballers))] 
        public FootballersDto[] Footballers { get; set; } = null!;
    }
}
