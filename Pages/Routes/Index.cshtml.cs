
using System.Security.Claims;
using Airflights.Models;
using Airflights.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Airflights.Pages.Route
{
    
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IFlightsService _flightsService;
        private readonly IUsersService _usersService;
        public IndexModel(IFlightsService flightsService,
                        IUsersService usersService)
        {
            _flightsService = flightsService;
            _usersService = usersService;
        }
        public List<FlightViewModel> Items {get; set;} = [];
        public string Message {get; set;} = string.Empty;
        private UserViewModel? UserView {get; set;}
        public async Task OnGetAsync()
        {
            Console.WriteLine("Страница c существующими рейсами");
            Items = await _flightsService.GetAllAsync();
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
        public string UserName
        {
            get
            {
                return UserView!=null ? UserView.Name : "Неизвестный пользоватлеь";
            }
        }
    }
}