
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Aircraft
{
    
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAircraftsService _aircraftsService;
        public IndexModel(IAircraftsService aircraftsService)
        {
            _aircraftsService = aircraftsService;
        }
        public List<AircraftViewModel> Items {get; set;} = [];
        public string Message {get; set;} = string.Empty;

        public async Task OnGetAsync()
        {
            Console.WriteLine("Страница c существующими рейсами");
            Items = await _aircraftsService.GetAllAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}