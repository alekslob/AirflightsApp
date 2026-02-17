
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
        private readonly IFlightsShedulerService _flightsShedulerService;

        public IndexModel(IFlightsShedulerService flightsShedulerService)
        {
            _flightsShedulerService = flightsShedulerService;
        }

        public List<FlightShedulerViewModel>? Items { get; set; }
        public string Message {get; set;} = string.Empty;

        public async Task OnGetAsync()
        {
            Console.WriteLine("Страница для сотрудниковв");
            Items = await _flightsShedulerService.GetAllAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}