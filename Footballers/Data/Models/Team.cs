using System.ComponentModel.DataAnnotations;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [RegularExpression(@"[a-zA-Z0-9\s\.\-]+")]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Nationality { get; set; } = null!;

        [Required]
        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers  { get; set; } 
            = new HashSet<TeamFootballer>();
    }
}
