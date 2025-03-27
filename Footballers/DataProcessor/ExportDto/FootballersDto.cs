using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    [XmlType("Footballer")]
    public class FootballersDto
    {
        [XmlElement(nameof(Name))]
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [XmlElement(nameof(ContractStartDate))]
        [Required] 
        public string ContractStartDate { get; set; } = null!;

        [XmlElement(nameof(ContractEndDate))]
        [Required] 
        public string ContractEndDate { get; set; } = null!;

        [XmlElement(nameof(BestSkillType))]
        [Required]
        [Range(0, 4)]
        public int BestSkillType { get; set; }

        [XmlElement(nameof(PositionType))]
        [Required]
        [Range(0, 3)]
        public int PositionType { get; set; }

    }
}