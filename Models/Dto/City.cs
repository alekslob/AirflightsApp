using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airflights.Models
{
    [Table("cities")]
    public class City
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(3)]
        public string? IadaCode { get; set; }
    }
}