using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airflights.Models
{
    [Table("aircraft_models")]
    public class AircraftModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
        [Required]
        public int TotalSeats {get; set;}
        public int Rows {get; set;}
        public int SeatsPerRow {get; set;}

    }
}