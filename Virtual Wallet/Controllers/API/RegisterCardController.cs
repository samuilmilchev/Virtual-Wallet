using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    public class RegisterCardController : Controller
    {
        [Route("api/card")]
        [ApiController]
        public class CardApiController : ControllerBase
        {
            private readonly IUsersService _usersService;
            private readonly ICardService _cardService;

            public CardApiController(IUsersService usersService, ICardService cardService)
            {
                _usersService = usersService;
                _cardService = cardService;
            }

            // POST: api/card/register
            [HttpPost("register")]
            public IActionResult Register([FromBody] CardViewModel cardViewModel)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Return 400 Bad Request if model validation fails
                }

                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." }); // Return 404 Not Found if user is not found
                }

                // Create the card based on the provided view model
                Card card = new Card
                {
                    CardNumber = cardViewModel.CardNumber,
                    ExpirationData = cardViewModel.ExpirationDate,
                    CardType = cardViewModel.CardType,
                    CheckNumber = HashCVV(cardViewModel.CheckNumber, GenerateSalt()),
                    UserId = user.Id,
                    User = user
                };

                // Save the card and associate it with the user
                _cardService.Create(card);
                _usersService.AddUserCard(card, user);

                return StatusCode(StatusCodes.Status201Created, card); // Return 201 Created with card data
            }

            // Utility method to generate salt
            private static string GenerateSalt()
            {
                byte[] saltBytes = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(saltBytes);
                }
                return Convert.ToBase64String(saltBytes);
            }

            // Hash the card CVV using SHA-256 with a salt
            private static string HashCVV(string cvv, string salt)
            {
                // Combine the CVV and the salt
                string saltedCVV = cvv + salt;

                // Use SHA256 to hash the combined string
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedCVV));
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }
    }
}
