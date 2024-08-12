using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.Controllers.MVC
{
    public class CardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            CardViewModel cardViewModel = new CardViewModel();

            return View(cardViewModel);
        }
    }
}
