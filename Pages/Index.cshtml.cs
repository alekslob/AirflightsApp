using Microsoft.AspNetCore.Mvc.RazorPages;
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Mvc;

namespace Airflights.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IFlightsShedulerService _flightsShedulerService;

        public IndexModel(IFlightsShedulerService flightsShedulerService)
        {
            _flightsShedulerService = flightsShedulerService;
        }

        public List<FlightShedulerViewModel> Items { get; set; } = [];

        public async Task OnGetAsync()
        {
            Items = await _flightsShedulerService.GetAllAsync();
        }
    }
}