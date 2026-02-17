
using System.Security.Claims;
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Staf
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public int? CitiesKey {get; set;}
        private readonly IFlightsShedulerService _flightsShedulerService;
        private readonly IDictionaryService _dictionaryService;

        public IndexModel(IFlightsShedulerService flightsShedulerService,
                        IDictionaryService dictionaryService)
        {
            _flightsShedulerService = flightsShedulerService;
            _dictionaryService = dictionaryService;
        }

        public List<FlightShedulerViewModel> Items { get; set; } = [];
        public string Message {get; set;} = string.Empty;
        
        public List<DictionaryBaseModel> Cities {get; set;} = [];

        public async Task OnGetAsync(int? citieskey = null)
        {
            CitiesKey=null;
            Console.WriteLine("Страница для сотрудниковв");
            Items = await _flightsShedulerService.GetAllAsync(cityId: citieskey);
            Cities = await _dictionaryService.GetCitiesAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}