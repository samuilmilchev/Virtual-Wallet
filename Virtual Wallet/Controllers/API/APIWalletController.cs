using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Virtual_Wallet.DTOs;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIWalletController : ControllerBase
    {

        private readonly IWalletService _walletService;
        private readonly IUsersService _usersService;

        public APIWalletController(IWalletService walletService, IUsersService usersService)
        {
            _walletService = walletService;
            _usersService = usersService;
        }

        /// <summary>
        /// Converts funds from one currency to another for the authenticated user.
        /// </summary>
        /// <param name="model">The conversion details including amount, from currency, and to currency.</param>
        /// <returns>Returns a success message if the conversion is successful.</returns>
        /// <response code="200">Funds converted successfully</response>
        /// <response code="400">Invalid model state</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/APIWallet/convert-funds
        ///     {
        ///        "amount": 100,
        ///        "fromCurrency": "USD",
        ///        "toCurrency": "EUR"
        ///     }
        ///
        /// </remarks>
        [HttpPost("convert-funds")]
        public IActionResult ConvertFunds([FromBody] ConvertFundsViewModel model)
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

                return Ok(new { message = "Funds converted successfully!" });
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Creates a saving wallet for the authenticated user.
        /// </summary>
        /// <param name="model">The saving wallet details.</param>
        /// <returns>Returns the created saving wallet model.</returns>
        /// <response code="200">Saving wallet created successfully</response>
        /// <response code="404">User not found</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/APIWallet/create-saving-wallet
        ///     {
        ///        "amount": 1000,
        ///        "interestRate": 0.05
        ///     }
        ///
        /// </remarks>
        [HttpPost("create-saving-wallet")]
        public IActionResult CreateSavingWallet([FromBody] SavingWalletViewModel model)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            model.CurrentUser = user;
            model.InterestRate = _walletService.CalculateInterest(model);
            model.StartDate = DateTime.Now;
            model.FinalAmount = _walletService.CalculateTotal(model);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Instead of using TempData, pass the model directly to the next step
            return Ok(model);
        }

        /// <summary>
        /// Reviews and creates a saving wallet based on user acceptance.
        /// </summary>
        /// <param name="request">The request to review saving wallet creation.</param>
        /// <returns>Returns a success message if the wallet is created.</returns>
        /// <response code="200">Saving wallet created successfully</response>
        /// <response code="400">Saving wallet creation rejected</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/APIWallet/review-saving-wallet
        ///     {
        ///        "text": "Accept",
        ///        "model": { "amount": 1000, "interestRate": 0.05 }
        ///     }
        ///
        /// </remarks>
        [HttpPost("review-saving-wallet")]
        public IActionResult ReviewSavingWallet([FromBody] ReviewSavingWalletRequest request)
        {
            if (request.Text == "Accept")
            {
                var model = request.Model;
                if (model != null)
                {
                    _walletService.CreateSavingWallet(model);
                    return Ok(new { message = "Saving wallet created successfully" });
                }
            }

            return BadRequest("Saving wallet creation rejected.");
        }

        /// <summary>
        /// Indicates a successful transaction.
        /// </summary>
        /// <returns>Returns a success message.</returns>
        /// <response code="200">Transaction successful</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/APIWallet/transaction-success
        ///
        /// </remarks>
        [HttpGet("transaction-success")]
        public IActionResult TransactionSuccess()
        {
            return Ok(new { message = "Transaction successful" });
        }

        [HttpGet("large-transaction-verification-form")]
        public IActionResult LargeTransactionVerificationForm()
        {
            // This endpoint doesn't seem necessary in an API context, but if needed:
            return Ok(new { message = "Large transaction verification form displayed" });
        }

        /// <summary>
        /// Verifies a transaction based on the provided transaction token.
        /// </summary>
        /// <param name="request">The transaction verification details.</param>
        /// <returns>Returns a success message if the transaction is verified.</returns>
        /// <response code="200">Transaction verified and completed successfully</response>
        /// <response code="400">Invalid or expired verification code</response>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/APIWallet/verify-transaction
        ///     {
        ///        "transactionToken": "abc123",
        ///        "senderUsername": "senderUser",
        ///        "recipientUsername": "recipientUser",
        ///        "currency": "USD",
        ///        "amount": 500
        ///     }
        ///
        /// </remarks>
        [HttpPost("verify-transaction")]
        public IActionResult VerifyTransaction([FromBody] VerifyTransactionRequest request)
        {
            if (string.IsNullOrEmpty(request.TransactionToken))
            {
                return BadRequest("Verification code is required.");
            }

            var senderUsername = request.SenderUsername;
            var recipientUsername = request.RecipientUsername;
            var sender = _usersService.GetByUsername(senderUsername);
            var recipient = _usersService.GetByUsername(recipientUsername);

            if (sender == null || recipient == null)
            {
                return BadRequest("Invalid request.");
            }

            if (sender.TransactionVerificationToken == request.TransactionToken && sender.TransactionTokenExpiry >= DateTime.Now)
            {
                var currency = (Currency)Enum.Parse(typeof(Currency), request.Currency);
                var senderWallet = sender.UserWallets.FirstOrDefault(s => s.Currency == currency);
                var recipientWallet = recipient.UserWallets.FirstOrDefault(x => x.Currency == currency);

                _walletService.TransferFunds(request.Amount, currency, senderWallet, recipientWallet, sender);

                // Clear the token after successful verification
                sender.TransactionVerificationToken = null;
                sender.TransactionTokenExpiry = null;
                _usersService.Update(sender.Id, sender);

                return Ok(new { message = "Transaction verified and completed successfully" });
            }
            else
            {
                return BadRequest("Invalid or expired verification code.");
            }
        }

    }
}
