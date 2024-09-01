using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;

        public HomeController(IUsersService usersService,IEmailService emailService)
        {
            _usersService = usersService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);
            }

            //_emailService.SendAsync("alexander_ng@abv.bg", "Confirm Email" , "Hi, Alex!");
            return View();
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}
