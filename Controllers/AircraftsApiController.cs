
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Airflights.Controllers
{
    [ApiController]
    [Route("api/aircrafts/")]
    public class AircraftsApiController: ControllerBase
    {
        private readonly IAircraftsService _aircraftsService;
        public AircraftsApiController(IAircraftsService aircraftsService)
        {
            _aircraftsService = aircraftsService;
        }
        /// <summary>
        /// Список доступных самолетов, их статус и положение
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<ActionResult<List<AircraftViewModel>>> GetAircraftsItems()
        {
            var aircraftItems = await _aircraftsService.GetAllAsync();
            return Ok(aircraftItems);
        }
        /// <summary>
        /// Добавление самолета в арсенал
        /// </summary>
        /// <param name="aircraftCreateModel"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<ActionResult> PostAircraft(AircraftCreateModel aircraftCreateModel)
        {
            try
            {
                await _aircraftsService.CreateAsync(aircraftCreateModel);
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }

        /// <summary>
        /// Изменение статуса самолета
        /// </summary>
        /// <param name="aircraftEditModel"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<ActionResult> PutAircraft(AircraftEditModel aircraftEditModel)
        {
            try
            {
                await _aircraftsService.UpdateAsync(aircraftEditModel);
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }
}