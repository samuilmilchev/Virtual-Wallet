using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly IUsersService _usersService;

        public WalletController(IWalletService walletService, IUsersService usersService)
        {
            _walletService = walletService;
            _usersService = usersService;
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

        [HttpGet]
        public IActionResult CreateSavingWallet()
        {
            SavingWalletViewModel model = new SavingWalletViewModel();

            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            model.CurrentUser = user;

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateSavingWallet(SavingWalletViewModel model)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            model.CurrentUser = user;

            model.InterestRate = _walletService.CalculateInterest(model);
            model.StartDate = DateTime.Now;
            model.FinalAmount = _walletService.CalculateTotal(model);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            TempData["SavingWalletModel"] = JsonConvert.SerializeObject(model, settings);

            return View("Review", model);
        }

        [HttpPost]
        public IActionResult Review(string text)
        {
            var model = new SavingWalletViewModel();

            if (text == "Accept")
            {
                if (TempData["SavingWalletModel"] != null)
                {
                    model = JsonConvert.DeserializeObject<SavingWalletViewModel>((string)TempData["SavingWalletModel"]);
                    _walletService.CreateSavingWallet(model);
                    return RedirectToAction("UserSavingWallets" , "User");
                }
            }

            return RedirectToAction("CreateSavingWallet");
        }

    }
}