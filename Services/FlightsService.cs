using Airflights.Data;
using Airflights.Models;
using Microsoft.EntityFrameworkCore;

namespace Airflights.Services
{
    public interface IFlightsService
    {
        Task<FlightViewModel?> GetByIdAsync(int id);
        Task<List<FlightViewModel>> GetAllAsync(bool? onlyActive = null);
        Task<Flight> CreateAsync(FlightCreateModel data);
        Task<Flight> UpdateAsync(FlightEditModel data);
    }

    public class FlightsService: IFlightsService
    {
        private readonly ApplicationDbContext _context;
        public FlightsService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        
        public async Task<FlightViewModel?> GetByIdAsync(int id)
        {
            return await _context.Flights.Select(
                flight => new FlightViewModel
                {
                    Id = flight.Id, 
                    Number = flight.Number,
                    ArrivalAirportCode = flight.ArrivalAirport.IadaCode,
                    DepartureAirportCode = flight.DepartureAirport.IadaCode,
                    ScheduledArrival = flight.ScheduledArrival,
                    ScheduledDeparture = flight.ScheduledDeparture,
                    Pattern = flight.Pattern,
                    IsActive = flight.IsActive
                }
            ).FirstOrDefaultAsync(f => f.Id == id);
        }
        public async Task<List<FlightViewModel>> GetAllAsync(bool? onlyActive = null)
        {
            return await _context.Flights
                .Where(f => onlyActive != null ? f.IsActive == onlyActive : true)
                .Select(
                    flight => new FlightViewModel
                    {
                        Id = flight.Id, 
                        Number = flight.Number,
                        ArrivalAirportCode = flight.ArrivalAirport.IadaCode,
                        DepartureAirportCode = flight.DepartureAirport.IadaCode,
                        Pattern = flight.Pattern,
                        IsActive = flight.IsActive
                    }
                ).ToListAsync();
        }
        private async Task<bool> CheckByNumber(string number)
        {
            return await _context.Flights.AnyAsync(f => f.Number == number);
        }
        public async Task<Flight> CreateAsync(FlightCreateModel data)
        {
            var isExistNumber = await CheckByNumber(data.Number);
            if (isExistNumber) throw new Exception($"Номер {data.Number} уже существует");
            var flight = new Flight
            {
                Number = data.Number,
                DepartureAirportId = data.DepartureAirportId,
                ArrivalAirportId = data.ArrivalAirportId,
                Pattern = (RecurrencePattern)data.PatternKey,
                ScheduledDeparture = data.ScheduledDeparture,
                ScheduledArrival = data.ScheduledArrival
            };
            _context.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }
        public async Task<Flight> UpdateAsync(FlightEditModel data)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == data.Id);
            if (flight == null) throw new Exception("Маршрута не сушествует");
            flight.IsActive = data.IsActive;
            _context.Update(flight);
            await _context.SaveChangesAsync();
            return flight;
        }
    }
}