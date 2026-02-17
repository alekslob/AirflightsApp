using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Airflights.Models
{
    public enum RecurrencePattern
    {
        [EnumMember(Value = "Daily")]
        Daily,
        [EnumMember(Value ="Weekly")]
        Weekly,
        [EnumMember(Value ="Specific")]
        Specific
    }

    [Table("flights")]
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(6)]
        public string Number {get; set;} = string.Empty;
        
        [Required]
        [ForeignKey(nameof(Airport))]
        public int DepartureAirportId {get; set;}
        [Required]
        [ForeignKey(nameof(Airport))]
        public int ArrivalAirportId {get; set;}
        public DateTime ScheduledDeparture {get; set;}
        public DateTime ScheduledArrival {get; set;}
        public RecurrencePattern Pattern {get;set;} = RecurrencePattern.Weekly;
        public bool IsActive {get; set;} = true;

        // [JsonIgnore] 
        public virtual Airport DepartureAirport { get; set; }
        // [JsonIgnore] 
        public virtual Airport ArrivalAirport { get; set; }
    }
}