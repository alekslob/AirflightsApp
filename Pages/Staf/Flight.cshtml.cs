
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Staf
{
    
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class FlightModel : PageModel
    {
        private readonly IFlightsShedulerService _flightsShedulerService;
        private readonly IFlightsService _flightsService;
        private readonly IDictionaryService _dictionaryService;

        public FlightModel(IFlightsShedulerService flightsShedulerService,
                            IFlightsService flightsService,
                            IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
            _flightsService = flightsService;
            _flightsShedulerService = flightsShedulerService;
        }
        public FlightShedulerViewModel? FlightShedule {get; set;}
        public Flight? FlightInfo {get; set;}
        public List<DictionaryBaseModel> FlightsStatuses {get; set;} = [];
        public List<DictionaryBaseModel> Aircrafts {get; set;} = [];
        public int Id { get; private set; }
        public async Task OnGetAsync(int id, bool showChoose = false, bool showStatus = false)
        {
            Id = id;
            ShowChooseAircrafts = showChoose;
            ShowChooseStatus = showStatus;
            FlightShedule = await _flightsShedulerService.GetByIdAsync(Id);
            // if (FlightShedule != null) FlightInfo = await _flightsService.GetByIdAsync(FlightShedule.FlightId);
            FlightsStatuses = (await _dictionaryService.GetFlightStatusesAsync()).Where(
                status => FlightShedule?.Status == FlightStatus.Scheduled ? new List<int>{0,1,4,5}.Contains(status.Key) : true
                && FlightShedule?.Status == FlightStatus.Boarding  ? new List<int>{1,2,4,5}.Contains(status.Key) : true
                && FlightShedule?.Status == FlightStatus.Departed ? status.Key==(int)FlightStatus.Arrived : true
                && FlightShedule?.Status == FlightStatus.Delayed ? new List<int>{0,1,4}.Contains(status.Key) : true
                && FlightShedule?.Status == FlightStatus.Cancelled ? new List<int>{0,1,4,5}.Contains(status.Key) : true
                ).ToList();
            Aircrafts = await _dictionaryService.GetAircraftsAsync(FlightShedule.DepartureAirportId);
        }
        
        public string Message { get; private set; } = "";
        
        public async Task OnPostAsync(int id, int statuskey, int aitkraftid)
        {
            // Message = $"Ваше имя: {statuskey}///{id}///{aitkraftid}";
            var data = new FlightShedulerEditModel
            {
                Id = id, AircraftId = aitkraftid, StatusKey=statuskey
            };
            await _flightsShedulerService.UpdateAsync(data);
            await OnGetAsync(id);
        }

        public bool ShowChooseAircrafts {get; set;} = false;
        public bool ShowChooseStatus {get; set;} = false;

        public bool CanEditAirflight
        {
            get
            {
                return FlightShedule?.Status == FlightStatus.Scheduled;
            }
        }
        public bool CanEditStatus
        {
            get
            {
                return FlightShedule?.Status != FlightStatus.Arrived;
            }
        }
        public bool CanSavedChenges
        {
            get
            {
                return CanEditAirflight || CanEditStatus;
            }
        }

    }
}