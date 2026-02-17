using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Aircraft
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class AircraftModel : PageModel
    {
        private readonly IAircraftsService _aircraftsService;
        private readonly IDictionaryService _dictionaryService;
        public AircraftModel(IAircraftsService aircraftsService,
                            IDictionaryService dictionaryService)
        {
            _aircraftsService=aircraftsService;
            _dictionaryService=dictionaryService;
        }
        public string Message { get; private set; } = "";
        public int Id { get; private set; }
        public AircraftViewModel? AircraftInfo {get; set;}
        public List<DictionaryBaseModel> AircraftStatuses { get; set; } = [];
        public bool ShowChooseStatus {get; set;} = false;
        public async Task OnGetAsync(int id, bool showChoose = false, bool showStatus = false)
        {
            Id = id;
            ShowChooseStatus = showStatus;
            AircraftInfo = await _aircraftsService.GetByIdAsync(id);
            AircraftStatuses = await _dictionaryService.GetAircraftsStatusesAsync();
        }
        public async Task OnPostAsync(int id, int statuskey)
        {
            // Message = $"Ваше имя: {statuskey}///{id}///";
            var data = new AircraftEditModel
            {
                Id = id, 
                StatusKey = statuskey
            };
            await _aircraftsService.UpdateAsync(data);
            await OnGetAsync(id);
        }
        public bool CanEditStatus
        {
            get
            {
                return true;
            }
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