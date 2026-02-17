
namespace Airflights.Models
{
    public class AircraftViewModel
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public int TotalSeats {get; set;}
        public string? AirportCode {get; set;}
        public AircraftsStatus Status {get; set;}

    }
    public class AircraftCreateModel
    {
        public string Name {get; set;} = string.Empty;
        public string Code {get; set;} = string.Empty;
        // public int TotalSeats{get; set;}
        public int ModelId {get; set;}
        public int AirportId {get; set;}
        public int StatusKey {get; set;}
    }
    public class AircraftEditModel
    {
        public int Id{ get; set;}
        public int StatusKey {get; set;}
    }
}