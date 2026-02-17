using Airflights.Services;
using Airflights.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airflights.Controllers
{
    [ApiController]
    [Route("api/dictionary")]
    public class DictionaryApiController: ControllerBase
    {
        private IDictionaryService _dictionaryService;
        public DictionaryApiController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        [HttpGet("aircrafts/statuses/")]
        public async Task<ActionResult<List<DictionaryBaseModel>>> GetAircraftStatuses()
        {
            var statuseItems = await _dictionaryService.GetAircraftsStatusesAsync();
            return Ok(statuseItems);
        }

        [HttpGet("aircrafts/models/")]
        public async Task<ActionResult<List<DictionaryBaseModel>>> GetAircraftModels()
        {
            var modelitems = await _dictionaryService.GetAircraftsModelsAsync();
            return Ok(modelitems);
        }

        [HttpGet("airports/")]
        public async Task<ActionResult<List<DictionaryBaseModel>>> GetAirports()
        {
            var aircraftitems = await _dictionaryService.GetAirportsAsync();
            return Ok(aircraftitems);
        }
    }
}