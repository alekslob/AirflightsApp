using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Aircraft
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class CreateModel: PageModel
    {
        private readonly IAircraftsService _aircraftsService;
        private readonly IDictionaryService _dictionaryService;
        public CreateModel(IAircraftsService aircraftsService,
                            IDictionaryService dictionaryService)
        {
            _aircraftsService=aircraftsService;
            _dictionaryService=dictionaryService;
        }
        public string Message {get; set;} = string.Empty;
        public List<DictionaryBaseModel> AircraftStatuses { get; set; } = [];
        public List<DictionaryBaseModel> AircraftModels { get; set; } = [];
        public List<DictionaryBaseModel> Airports { get; set; } = [];

        public async Task OnGetAsync()
        {
            AircraftStatuses = await _dictionaryService.GetAircraftsStatusesAsync();
            AircraftModels = await _dictionaryService.GetAircraftsModelsAsync();
            Airports = await _dictionaryService.GetAirportsAsync();
        }
        public async Task<IActionResult> OnPostAsync(int modelkey, string aircraftname, string aircraftcode, int airportkey, int statuskey)
        {
            Message = $"Сохранение: {modelkey}///{aircraftname}///{airportkey}///{statuskey}///";
            var data = new AircraftCreateModel
            {
                Name = aircraftname,
                Code = aircraftcode,
                ModelId = modelkey,
                AirportId = airportkey,
                StatusKey = statuskey
            };
            await _aircraftsService.CreateAsync(data);
            return Redirect("/Aircrafts");
            
        }
    }
}