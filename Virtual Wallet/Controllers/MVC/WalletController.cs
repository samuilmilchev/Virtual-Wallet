using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public IActionResult ConvertFunds()
        {
            ConvertFundsViewModel model = new ConvertFundsViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ConvertFunds(ConvertFundsViewModel model)
        {


            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                _walletService.ConvertFunds(
                    model.Amount,
                    model.FromCurrency,
                    model.ToCurrency,
                    username
                );

                // Optionally, add a success message or redirect to another page.
                ViewBag.Message = "Funds converted successfully!";
            }

            //If the model is not valid, return to the same view with validation errors.
            return View(model);
        }
    }
}