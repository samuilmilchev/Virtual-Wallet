using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Virtual_Wallet.Models.Entities;
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

        public IActionResult TransactionSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LargeTransactionVerificationForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyTransaction(string transactionToken)
        {
            if (string.IsNullOrEmpty(transactionToken))
            {
                ViewData["ErrorMessage"] = "Verification code is required.";
                return View("LargeTransactionVerificationForm"); // Return to the view with an error message
            }

            if (!TempData.ContainsKey("Amount") ||
                !TempData.ContainsKey("Currency") ||
                //!TempData.ContainsKey("SenderWalletId") ||
                //!TempData.ContainsKey("RecipientWalletId") ||
                !TempData.ContainsKey("SenderUsername") ||
                !TempData.ContainsKey("RecipientUsername"))
            {
                ViewData["ErrorMessage"] = "Invalid transaction data.";
                return View("LargeTransactionVerificationForm");
            }

            var amount = Convert.ToDecimal(TempData["Amount"]);
            var currency = (Currency)Enum.Parse(typeof(Currency), TempData["Currency"].ToString());
            //var senderWalletId = Convert.ToInt32(TempData["SenderWalletId"]);
            //var recipientWalletId = Convert.ToInt32(TempData["RecipientWalletId"]);
            var senderUsername = TempData["SenderUsername"] as string;
            var recipientUsername = TempData["RecipientUsername"] as string;

            var sender = _usersService.GetByUsername(senderUsername);
            var recipient = _usersService.GetByUsername(recipientUsername);


            if (sender == null || string.IsNullOrEmpty(transactionToken))
            {
                ViewData["ErrorMessage"] = "Invalid request.";
                return View("LargeTransactionVerificationForm");
            }

            if (recipient == null || string.IsNullOrEmpty(transactionToken))
            {
                ViewData["ErrorMessage"] = "Invalid request.";
                return View("LargeTransactionVerificationForm");
            }


            if (sender.TransactionVerificationToken == transactionToken && sender.TransactionTokenExpiry >= DateTime.Now)
            {

                var senderWallet = sender.UserWallets.FirstOrDefault(s =>s.Currency == currency);
                var recipientWallet = recipient.UserWallets.FirstOrDefault(x => x.Currency == currency);

                _walletService.TransferFunds(amount , currency , senderWallet , recipientWallet, sender);
                // Clear the token after successful verification
                sender.TransactionVerificationToken = null;
                sender.TransactionTokenExpiry = null;

                _usersService.Update(sender.Id, sender); 

                return RedirectToAction("TransactionSuccess"); 
            }
            else
            {
                // Invalid token or token expired
                ViewData["ErrorMessage"] = "Invalid or expired verification code.";
                return View(); // Return to the view with an error message
            }
        }
    }
}