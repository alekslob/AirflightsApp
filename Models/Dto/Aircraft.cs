using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Airflights.Models
{
    public enum AircraftsStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        
        [EnumMember(Value = "Maintenance")]
        Maintenance,
        
        [EnumMember(Value = "Retired")]
        Retired
    }

    [Table("aircrafts")]
    public class Aircraft
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [ForeignKey(nameof(AircraftModel))]
        public int ModelId {get; set;}
        [Required]
        [MaxLength(100)]
        public string Code {get; set;} = string.Empty;
        [Required]
        [ForeignKey(nameof(Airport))]
        public int AirportId {get; set;}
        public AircraftsStatus Status {get; set;} = AircraftsStatus.Active;

        
        [JsonIgnore] 
        public virtual Airport Airport { get; set; }
        
        [JsonIgnore] 
        public virtual AircraftModel Model { get; set; }

    }

}