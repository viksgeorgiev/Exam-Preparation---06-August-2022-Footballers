using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class Footballer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime ContractStartDate  { get; set; }

        [Required]
        public DateTime ContractEndDate  { get; set; }

        [Required]
        [Range(0, 3)]
        public PositionType PositionType { get; set; }

        [Required]
        [Range(0, 4)]
        public BestSkillType BestSkillType { get; set; }

        [Required]
        [ForeignKey(nameof(Coach))]
        public int CoachId { get; set; }
        public virtual Coach Coach { get; set; } = null!;

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } 
            = new HashSet<TeamFootballer>();
    }
}
