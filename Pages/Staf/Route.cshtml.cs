using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Route
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class RouteModel : PageModel
    {
        private readonly IFlightsService _flightsService;
        public RouteModel(IFlightsService flightsService)
        {
            _flightsService=flightsService;
        }
        public string Message { get; private set; } = "";
        public int Id { get; private set; }
        public FlightViewModel? FlightInfo{get; set;}
        public bool ShowChooseAircrafts {get; set;} = false;
        public bool ShowChooseStatus {get; set;} = false;
        public async Task OnGetAsync(int id, bool showChoose = false, bool showStatus = false)
        {
            Id = id;
            ShowChooseAircrafts = showChoose;
            ShowChooseStatus = showStatus;
            FlightInfo = await _flightsService.GetByIdAsync(id);
        }
        public async Task OnPostAsync(int id, bool status)
        {
            // Message = $"Ваше имя: {status}///{id}///";
            var data = new FlightEditModel
            {
                Id = id, 
                IsActive = status
            };
            await _flightsService.UpdateAsync(data);
            await OnGetAsync(id);
        }
        public bool CanSavedChenges
        {
            get
            {
                return true;//CanEditAirflight || CanEditStatus;
            }
        }

    }
}