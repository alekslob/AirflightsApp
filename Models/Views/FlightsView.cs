namespace Airflights.Models
{
    public class FlightViewModel
    {
        public int Id {get; set;}
        public string Number {get; set;} = string.Empty;
        public string DepartureAirportCode {get; set;} = string.Empty;
        public string ArrivalAirportCode {get;set;} = string.Empty;
        public string ArrivalAirportName {get; set;} = string.Empty;
        public DateTime ScheduledDeparture {get;set;}
        public DateTime ScheduledArrival {get;set;}
        public RecurrencePattern Pattern {get; set;} = RecurrencePattern.Daily;
        public bool IsActive {get; set;}
    }

    public class FlightCreateModel
    {
        public string Number {get; set;} = string.Empty;
        public int DepartureAirportId {get; set;}
        public int ArrivalAirportId {get; set;}
        public int PatternKey {get; set;}
        public DateTime ScheduledDeparture {get;set;}
        public DateTime ScheduledArrival {get;set;}
    }

    public class FlightEditModel
    {
        public int Id{get; set;}
        public bool IsActive {get;set;} 
    }
}