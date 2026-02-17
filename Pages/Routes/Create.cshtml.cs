using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Route
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class CreateModel: PageModel
    {
        [BindProperty]
        public string FlightName { get; set; } = "";
        
        [BindProperty]
        public int AirportDepartureKey { get; set; }
        
        [BindProperty]
        public int AirportArrivalKey { get; set; }
        
        [BindProperty]
        public int PatternKey { get; set; }
        
        [BindProperty]
        public string TimeDepartured { get; set; } = "";
        
        [BindProperty]
        public string TimeArrival { get; set; } = "";
        
        private readonly IFlightsService _flightsService;
        private readonly IDictionaryService _dictionaryService;
        public CreateModel(IFlightsService flightsService,
                            IDictionaryService dictionaryService)
        {
            _flightsService=flightsService;
            _dictionaryService=dictionaryService;
        }
        public string Message {get; set;} = string.Empty;
        public List<DictionaryBaseModel> Airports {get; set;} = [];
        public List<DictionaryBaseModel> FlightsPatterns {get; set;} = [];

        public async Task OnGetAsync()
        {
            Airports = await _dictionaryService.GetAirportsAsync();
            FlightsPatterns = await _dictionaryService.GetFlightPatternsAsync();
        }
        public async Task<IActionResult> OnPostAsync(string flightname, int airportdeparturekey, int airportarrivalkey, int patternkey, DateTime timedepartured, DateTime timearrival)
        {
            Message = $"Сохранение: {flightname}///{airportdeparturekey}///{airportarrivalkey}///{patternkey}///{timearrival}///";
            
            try
            {
                if (airportarrivalkey == airportdeparturekey) throw new Exception("Пунк вылета и прилета не может ыбть одинаковый");
                var data = new FlightCreateModel
                {
                    Number = flightname,
                    DepartureAirportId = airportdeparturekey,
                    ArrivalAirportId = airportarrivalkey,
                    PatternKey = patternkey,
                    ScheduledDeparture = timedepartured,
                    ScheduledArrival = timearrival <= timedepartured ? timearrival.AddDays(-1) : timearrival,
                };
                await _flightsService.CreateAsync(data);
                return Redirect("/Routes");
            } catch (Exception ex)
            {
                Message = ex.Message;
                await OnGetAsync();
                return Page();
            }
        }
    }
}