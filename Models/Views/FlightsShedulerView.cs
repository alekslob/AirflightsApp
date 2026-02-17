using Microsoft.AspNetCore.Routing.Constraints;

namespace Airflights.Models
{
    public class FlightShedulerViewModel
    {
        public int Id {get;set;}
        public string Number {get; set;} = string.Empty;
        public int DepartureAirportId {get; set;}
        public string DepartureAirportCode {get; set;} = string.Empty;
        public string ArrivalAirportCode {get; set;} = string.Empty;
        public DateTime DepartureTime {get; set;}
        public DateTime ArrivalTime {get; set;}
        public string? AircraftName {get; set;}
        public int AvailableSeats {get; set;}
        public FlightStatus Status {get;set;} = FlightStatus.Scheduled;
    }
    public class FlightsShedulerCreateModel
    {
        public int FlightId {get; set;}
        public int StatusKey {get; set;} = (int)FlightStatus.Scheduled;
        public DateTime DepartureTime {get; set;}
        public DateTime ArrivalTime {get; set;}
    }
    public class FlightShedulerEditModel
    {
        public int Id{get; set;}
        public int AircraftId {get; set;}
        public int StatusKey{get; set;}
    }
}