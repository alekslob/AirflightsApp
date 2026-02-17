using Airflights.Data;
using Airflights.Models;
using Microsoft.EntityFrameworkCore;

namespace Airflights.Services
{
    public interface IFlightsShedulerService
    {
        Task<FlightShedulerViewModel?> GetByIdAsync(int id);
        Task<List<FlightShedulerViewModel>> GetAllAsync(FlightStatus? status = null, DateTime? DepartureTimeStart = null, DateTime? DepartureTimeEnd = null, int? FlightId = null);
        Task<FlightShedule> CreateAsync(FlightsShedulerCreateModel data);
        Task<FlightShedule> UpdateAsync(FlightShedulerEditModel data);
    }

    public class FlightsShedulerService: IFlightsShedulerService
    {
        private readonly ApplicationDbContext _context;
        public FlightsShedulerService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<FlightShedulerViewModel?> GetByIdAsync(int id)
        {
            return await _context.FlightShedules.Select(
                flight => new FlightShedulerViewModel
                {
                    Id = flight.Id, 
                    Number = flight.Flight.Number,
                    DepartureAirportId = flight.Flight.DepartureAirportId,
                    AircraftName = flight.Aircraft != null ? flight.Aircraft.Name : "N/A", 
                    ArrivalAirportCode = flight.Flight.ArrivalAirport != null ? flight.Flight.ArrivalAirport.IadaCode : "N/A",
                    DepartureAirportCode = flight.Flight.DepartureAirport != null ? flight.Flight.DepartureAirport.IadaCode : "N/A",
                    ArrivalTime = flight.ActualArrival,
                    DepartureTime = flight.ActualDeparture,
                    AvailableSeats = 0,
                    Status = flight.Status
                }
            ).FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<List<FlightShedulerViewModel>> GetAllAsync(FlightStatus? status = null, DateTime? DepartureTimeStart = null, DateTime? DepartureTimeEnd = null, int? FlightId = null)
        {
            var res = await _context.FlightShedules.Where(f => 
                                                        status != null ? f.Status == status : true &&
                                                        DepartureTimeStart != null ? f.ActualDeparture >= DepartureTimeStart : true &&
                                                        DepartureTimeEnd != null ? f.ActualDeparture <= DepartureTimeEnd : true &&
                                                        FlightId != null ? f.FlightId ==FlightId : true
                                                        ).OrderByDescending(f => f.ActualDeparture).Select(
                flight => new FlightShedulerViewModel
                {
                    Id = flight.Id, 
                    Number = flight.Flight.Number,
                    AircraftName = flight.Aircraft != null ? flight.Aircraft.Name : null, //"N/A", 
                    ArrivalAirportCode = flight.Flight.ArrivalAirport != null ? flight.Flight.ArrivalAirport.IadaCode : "", //"N/A", 
                    DepartureAirportCode = flight.Flight.DepartureAirport != null ? flight.Flight.DepartureAirport.IadaCode : "", //"N/A", 
                    ArrivalTime = flight.ActualArrival,
                    DepartureTime = flight.ActualDeparture,
                    AvailableSeats = flight.Aircraft != null ? flight.Aircraft.Model.TotalSeats - flight.OccupiedSeats : 0,
                    Status = flight.Status
                }
            ).ToListAsync();
            return res;
        }
        private async Task<bool> CheckFlightSedulerSameDay(int flightId, DateTime day)
        {
            return await _context.FlightShedules.AnyAsync(f => f.FlightId==flightId && f.ActualDeparture.Date == day.Date);
        }
        public async Task<FlightShedule> CreateAsync(FlightsShedulerCreateModel data)
        {
            var hasFlight = await CheckFlightSedulerSameDay(data.FlightId, data.ArrivalTime);
            if (hasFlight) throw new Exception("Уже существует рейс на этот день");
            var flightShedule = new FlightShedule
            {
                FlightId = data.FlightId,
                Status = (FlightStatus)data.StatusKey,
                ActualArrival = data.ArrivalTime,
                ActualDeparture = data.DepartureTime
            };
            _context.Add(flightShedule);
            await _context.SaveChangesAsync();
            return flightShedule;
        }
        public async Task<FlightShedule> UpdateAsync(FlightShedulerEditModel data)
        {
            var flightShedule = await _context.FlightShedules.FirstOrDefaultAsync(f => f.Id == data.Id);
            if (flightShedule == null) throw new Exception("Не найден рейс");
            if (data.AircraftId!=0){
                var aircraft = await _context.Aircrafts.FirstOrDefaultAsync(f => f.Id == data.AircraftId);
                if (aircraft == null) throw new Exception("Не найден самолет");
                flightShedule.AircraftId = aircraft.Id;
                var aircraftModel = await _context.AircraftModels.FirstAsync(m => m.Id == aircraft.ModelId);
                flightShedule.OccupiedSeats = new Random().Next(0, aircraftModel.TotalSeats + 1);
            }
            
            flightShedule.Status = (FlightStatus)data.StatusKey;
            _context.Update(flightShedule);
            if (flightShedule.Status == FlightStatus.Departed) flightShedule.ActualDeparture = DateTime.Now;
            if (flightShedule.Status == FlightStatus.Arrived)
            {
                var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == flightShedule.FlightId);
                if (flight == null) throw new Exception("Не найден маршрут");
                var aircraft = await _context.Aircrafts.FirstOrDefaultAsync(a => a.Id == flightShedule.AircraftId);
                if(aircraft!=null) {
                    aircraft.AirportId = flight.ArrivalAirportId;
                    _context.Update(aircraft);
                }
                flightShedule.ActualArrival=DateTime.Now;
            }
            await _context.SaveChangesAsync();
            
            return flightShedule;
        }
    }
}