using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;

        public HomeController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);
            }
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}
