using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Airflights.Models
{
    [Table("airports")]
    public class Airport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(3)]
        public string IadaCode { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(City))]  // Указываем внешний ключ
        public int CityId {get; set;}

        [JsonIgnore] 
        public virtual City City { get; set; }
    }
}