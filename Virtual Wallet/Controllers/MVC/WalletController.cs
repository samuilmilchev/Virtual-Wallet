using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services;

namespace Virtual_Wallet.Controllers.MVC
{
    public class WalletController : Controller
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public IActionResult ConvertFunds(ConvertFundsViewModel model)
        {
            if (ModelState.IsValid)
            {
                _walletService.ConvertFunds(
                    model.Amount,
                    model.FromCurrency,
                    model.ToCurrency,
                    model.FromWallet,
                    model.ToWallet,
                    model.User
                );

                // Optionally, add a success message or redirect to another page.
                ViewBag.Message = "Funds converted successfully!";
            }

            // If the model is not valid, return to the same view with validation errors.
            return View(model);
        }
    }
}