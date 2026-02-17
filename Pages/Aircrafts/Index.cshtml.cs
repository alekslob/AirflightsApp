
using System.Security.Claims;
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
        private readonly IUsersService _usersService;
        public IndexModel(IAircraftsService aircraftsService,
                            IUsersService usersService)
        {
            _aircraftsService = aircraftsService;
            _usersService = usersService;
        }
        public List<AircraftViewModel> Items {get; set;} = [];
        public string Message {get; set;} = string.Empty;
        private UserViewModel? UserView {get; set;}

        public async Task OnGetAsync()
        {
            Console.WriteLine("Страница c существующими рейсами");
            Items = await _aircraftsService.GetAllAsync();
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"); 
            UserView = await _usersService.GetById(userid);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
        public bool CanCreate
        {
            get
            {

                return UserView?.Role == UserRoles.Admin;
            }
        }
    }
}