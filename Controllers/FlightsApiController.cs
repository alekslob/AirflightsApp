using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Annotations;

namespace Airflights.Controllers
{
    [ApiController]
    [Route("api/flights")]
    public class FlightsApiController: ControllerBase
    {
        private readonly IFlightsShedulerService _flightsShedulerService;
        private readonly IFlightsService _flightsService;
        public FlightsApiController(IFlightsShedulerService flightsShedulerService,
                                    IFlightsService flightsService)
        {
            _flightsShedulerService = flightsShedulerService;
            _flightsService = flightsService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<FlightShedulerViewModel>>> GetFlightsItems()
        {
            var items = await _flightsShedulerService.GetAllAsync();
            return Ok(items);
        }

        [HttpPut()]
        public async Task<ActionResult> GetFlightsItems(FlightShedulerEditModel data)
        {
            try
            {
                await _flightsShedulerService.UpdateAsync(data);
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
        [HttpGet("routers")]
        public async Task<ActionResult<List<FlightViewModel>>> GetRourerItems()
        {
            var items = await _flightsService.GetAllAsync();
            return Ok(items);
        }
        [HttpPost("routers")]
        public async Task<ActionResult> PostRouter(FlightCreateModel data)
        {
            try
            {
                var flight = await _flightsService.CreateAsync(data);
                var flightSheduler = new FlightsShedulerCreateModel{
                    FlightId=flight.Id,
                    ArrivalTime=flight.ScheduledArrival,
                    DepartureTime=flight.ScheduledDeparture
                };
                if (flight.Pattern == RecurrencePattern.Specific)
                {
                    await _flightsShedulerService.CreateAsync(flightSheduler);
                }
                if (flight.Pattern == RecurrencePattern.Daily)
                {
                    for (int day=0; day<7; day++)
                    {
                        flightSheduler.ArrivalTime = flight.ScheduledArrival.AddDays(day);
                        flightSheduler.DepartureTime = flight.ScheduledDeparture.AddDays(day);
                        await _flightsShedulerService.CreateAsync(flightSheduler);
                    }
                }
                if (flight.Pattern == RecurrencePattern.Weekly)
                {
                    for (int week=0; week<3; week++)
                    {
                        flightSheduler.ArrivalTime = flight.ScheduledArrival.AddDays(week*7);
                        flightSheduler.DepartureTime = flight.ScheduledDeparture.AddDays(week*7);
                        await _flightsShedulerService.CreateAsync(flightSheduler);
                    }
                }
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
        [HttpPut("routers")]
        public async Task<ActionResult> PutRouter(FlightEditModel data)
        {
            try
            {
                await _flightsService.UpdateAsync(data);
                if (!data.IsActive)
                {
                    var items = await _flightsShedulerService.GetAllAsync(status: FlightStatus.Scheduled);
                    foreach (var item in items)
                    {
                        var flightShedulerEdit = new FlightShedulerEditModel{Id=item.Id, StatusKey = (int)FlightStatus.Cancelled};
                        await _flightsShedulerService.UpdateAsync(flightShedulerEdit);
                    }
                }
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(new { Error = e.Message });
            }
        }
    }

}