using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Airflights.Models
{
    public enum FlightStatus
    {
        [EnumMember(Value ="Sheduled")]
        Scheduled,
        [EnumMember(Value ="Boarding")]
        Boarding,
        [EnumMember(Value ="Departed")]
        Departed,
        [EnumMember(Value ="Arrived")]
        Arrived,
        [EnumMember(Value ="Delayed")]
        Delayed,
        [EnumMember(Value ="Cancelled")]
        Cancelled,
    }

    [Table("flights_shedule")]
    public class FlightShedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
        [Required]
        [ForeignKey(nameof(Flight))]
        public int FlightId {get; set;}

        
        [ForeignKey(nameof(Aircraft))]
        public int? AircraftId {get;set;} = null;
        public DateTime ActualDeparture {get;set;}
        public DateTime ActualArrival {get;set;}
        public FlightStatus Status {get;set;} = FlightStatus.Scheduled;

        public int OccupiedSeats {get; set;}

        
        [JsonIgnore] 
        public virtual Flight Flight { get; set; }
        [JsonIgnore] 
        public virtual Aircraft Aircraft { get; set; }
        

    }

}