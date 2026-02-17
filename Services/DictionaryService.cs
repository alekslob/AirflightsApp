
using Airflights.Data;
using Airflights.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace Airflights.Services
{
    public interface IDictionaryService
    {
        Task<List<DictionaryBaseModel>> GetAircraftsStatusesAsync();
        Task<List<DictionaryBaseModel>> GetAircraftsAsync(int airport_id);
        Task<List<DictionaryBaseModel>> GetAircraftsModelsAsync();
        Task<List<DictionaryBaseModel>> GetAirportsAsync();
        Task<List<DictionaryBaseModel>> GetFlightStatusesAsync();
        Task<List<DictionaryBaseModel>> GetFlightPatternsAsync();
    }

    public class DictionaryService: IDictionaryService
    {
        private readonly ApplicationDbContext _context;
        public DictionaryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<DictionaryBaseModel>> GetAircraftsStatusesAsync()
        {
            return Enum.GetValues(typeof(AircraftsStatus))
            .Cast<AircraftsStatus>()
            .Select(status => new DictionaryBaseModel{
                Key = (int)status,
                Value = status.ToString()
            }).ToList();
        }
        public async Task<List<DictionaryBaseModel>> GetAircraftsAsync(int airport_id)
        {
            return await _context.Aircrafts.Where(a => a.AirportId == airport_id && a.Status == AircraftsStatus.Active).Select(
                model => new DictionaryBaseModel
                {
                    Key = model.Id,
                    Value = model.Name
                }
            ).ToListAsync();
        }
        public async Task<List<DictionaryBaseModel>> GetAircraftsModelsAsync()
        {
            return await _context.AircraftModels.Select(
                model => new DictionaryBaseModel
                {
                    Key=model.Id,
                    Value=model.Name
                }
            ).ToListAsync();
        }
        public async Task<List<DictionaryBaseModel>> GetAirportsAsync()
        {
            return await _context.Airports.Select(
                airport => new DictionaryBaseModel
                {
                    Key=airport.Id,
                    Value=airport.IadaCode
                }
            ).ToListAsync();
        }
        public async Task<List<DictionaryBaseModel>> GetFlightStatusesAsync()
        {
            return Enum.GetValues(typeof(FlightStatus))
            .Cast<FlightStatus>()
            .Select(status => new DictionaryBaseModel
            {
                Key = (int)status,
                Value=status.ToString()
            }).ToList();
        }
        public async Task<List<DictionaryBaseModel>> GetFlightPatternsAsync()
        {
            return Enum.GetValues(typeof(RecurrencePattern))
            .Cast<RecurrencePattern>()
            .Select(pattern => new DictionaryBaseModel
            {
                Key = (int)pattern,
                Value=pattern.ToString()
            }).ToList();
        }
    }
}