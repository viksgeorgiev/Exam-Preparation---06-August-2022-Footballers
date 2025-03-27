namespace Footballers.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportTeamsDto
    {
        
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

       
        [Required]
        public int[] Footballers { get; set; } = Array.Empty<int>();
    }
}
