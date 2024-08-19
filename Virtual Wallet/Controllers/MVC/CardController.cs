using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class CardController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ICardService _cardService;

        public CardController(IUsersService usersService, ICardService cardService)
        {
            _usersService = usersService;
            _cardService = cardService;
        }

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

        [HttpPost]
        public IActionResult Register(CardViewModel cardViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(cardViewModel);
            }

            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            Card card = new Card();
            

           // card.CardHolder.Username = cardViewModel.CardHolder; //закоментирано за момента тъй като модела Карта няма вече string CardHolder 
            card.CardNumber = cardViewModel.CardNumber;             // a е User CardHolder
            card.ExpirationData = cardViewModel.ExpirationDate;
            card.CardType = cardViewModel.CardType;
            card.CheckNumber = HashCVV(cardViewModel.CheckNumber, GenerateSalt());
            card.UserId = user.Id;
            card.User = user;

            _cardService.Create(card);
            _usersService.AddUserCard(card, user);

            return View("Successful");
        }

        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Hash the card CVV using SHA-256 with a salt
        public static string HashCVV(string cvv, string salt)
        {
            // Combine the cvv and the salt
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
