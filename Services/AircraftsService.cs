
using System.ComponentModel.DataAnnotations;
using System.Text;
using Airflights.Data;
using Airflights.Models;
using Microsoft.EntityFrameworkCore;
using YamlDotNet.Core.Tokens;

namespace Airflights.Services
{
    public interface IAircraftsService
    {
        Task<AircraftViewModel?> GetByIdAsync(int id);
        Task<List<AircraftViewModel>> GetAllAsync();
        Task CreateAsync(AircraftCreateModel data);
        Task UpdateAsync(AircraftEditModel data);
    }

    public class AircraftsService: IAircraftsService
    {
        private readonly ApplicationDbContext _context;
        public AircraftsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AircraftViewModel?> GetByIdAsync(int id)
        {
            return await _context.Aircrafts.Select(
                aircraft => new AircraftViewModel
                {
                    Id = aircraft.Id,
                    Name = aircraft.Name,
                    TotalSeats = aircraft.Model.TotalSeats,
                    Status = aircraft.Status,
                    AirportCode = aircraft.Airport.IadaCode
                }
            ).FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<List<AircraftViewModel>> GetAllAsync()
        {
            return await _context.Aircrafts.Select(
                aircraft => new AircraftViewModel
                {
                    Id = aircraft.Id,
                    Name = aircraft.Name,
                    TotalSeats = aircraft.Model.TotalSeats,
                    Status = aircraft.Status,
                    AirportCode = aircraft.Airport.IadaCode
                }
            ).ToListAsync();
        }        

        private async Task<bool> CheckByCode(string code)
        {
            return await _context.Aircrafts.AnyAsync(a => a.Code == code);
        }
        public async Task CreateAsync(AircraftCreateModel data)
        {
            var isExistCode = await CheckByCode(data.Code);
            if (isExistCode) throw new Exception($"Самолет с кодом {data.Code} уже существует");
            var aircraft = new Aircraft
            {
                Name = data.Name,
                Code = data.Code,
                Status = (AircraftsStatus)data.StatusKey,
                ModelId = data.ModelId,
                AirportId = data.AirportId
            };
            _context.Add(aircraft);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(AircraftEditModel data)
        {
            var aircraft = await _context.Aircrafts.FirstOrDefaultAsync(a => a.Id == data.Id);
            if (aircraft == null) throw new Exception("Самолета не существует");
            aircraft.Status = (AircraftsStatus)data.StatusKey;
            _context.Update(aircraft);
            await _context.SaveChangesAsync();
        }
        
    }
}