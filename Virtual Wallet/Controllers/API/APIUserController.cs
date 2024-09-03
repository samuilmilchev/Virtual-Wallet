using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Virtual_Wallet.DTOs.TransactionDTOs;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Helpers.Contracts;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUserController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly IModelMapper _modelMapper;
        private readonly IWalletService _walletService;
        private readonly IPhotoService _photoService;
        private readonly ITransactionService _transactionService;
        private readonly IEmailService _emailService;

        public APIUserController(IUsersService usersService, IConfiguration configuration, IModelMapper modelMapper, IWalletService walletService, ITransactionService transactionService, IPhotoService photoService, IEmailService emailService)
        {
            _usersService = usersService;
            _configuration = configuration;
            _modelMapper = modelMapper;
            _walletService = walletService;
            _photoService = photoService;
            _transactionService = transactionService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CombinedUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var registerModel = model.Register;
                var walletModel = model.Wallet;

                CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    Email = registerModel.Email,
                    Username = registerModel.Username,
                    PhoneNumber = registerModel.PhoneNumber,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserWallets = new List<Wallet>(),
                    Role = UserRole.User
                };

                if (registerModel.Image != null)
                {
                    var result = await _photoService.UploadImageAsync(registerModel.Image);
                    if (result.Error != null)
                    {
                        return BadRequest("An error occurred while uploading the image");
                    }
                    user.Image = result.Url.ToString();
                }

                var wallet = new Wallet
                {
                    WalletName = walletModel.WalletName,
                    Owner = user,
                    Currency = Currency.BGN
                };

                user.UserWallets.Add(wallet);
                User createdUser = _usersService.Create(user);

                var responseDTO = _modelMapper.MapUser(createdUser);
                string token = CreateToken(user);

                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Change to true in production
                    SameSite = SameSiteMode.Strict
                });

                await _usersService.SendConfirmationEmailAsync(user);

                return Ok(new { message = "Registration successful. Please check your email to verify your account.", user = responseDTO });
            }

            return BadRequest(ModelState);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CombinedUserDTO model)
        {
            var loginRequest = model.Login;
            var user = _usersService.GetByUsername(loginRequest.Username);

            if (user == null || !VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Invalid credentials");

            if (!user.IsEmailVerified)
                return Unauthorized("Email not confirmed. Please check your email to confirm your account.");

            string token = CreateToken(user);

            HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // Change to true in production
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Login successful", token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logout successful" });
        }

        [HttpGet("details/{username}")]
        public IActionResult UserDetails(string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            var model = new UserViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Image = user.Image,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                IsBlocked = user.IsBlocked,
                Cards = user.Cards,
                AdminVerified = user.AdminVerified
            };

            return Ok(model);
        }

        [HttpPost("upload-photos-verification")]
        public async Task<IActionResult> UploadPhotosVerification([FromBody] VerifyUserViewModel model)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            if (user == null)
                return NotFound("User not found");

            var selfieResult = await _photoService.UploadImageAsync(model.Selfie);
            var idPhotoResult = await _photoService.UploadImageAsync(model.IdPhoto);

            if (selfieResult.Error != null || idPhotoResult.Error != null)
                return BadRequest("An error occurred while uploading photos");

            user.Selfie = selfieResult.Url.ToString();
            user.IdPhoto = idPhotoResult.Url.ToString();

            _usersService.UploadPhotoVerification(user.Selfie, user.IdPhoto, user);

            return Ok(new { message = "Photos uploaded successfully" });
        }

        [HttpGet("verify-users")]
        public IActionResult VerifyUsers()
        {
            var verifications = _usersService.GetAllVereficationApplies().Select(x => _modelMapper.Map(x)).ToList();
            return Ok(verifications);
        }

        [HttpPost("verify-users")]
        public IActionResult VerifyUsers([FromBody] string text)
        {
            string[] tokens = text.Split(',');
            string verificationValue = tokens[0];
            var user = _usersService.GetByUsername(tokens[1]);

            if (user == null)
                return NotFound("User not found");

            _usersService.UpdateUserVerification(user, verificationValue);
            return Ok(new { message = "User verification updated successfully" });
        }

        [HttpPost("assign-role")]
        public IActionResult AssignRole([FromBody] AssignRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _usersService.GetByUsername(model.Username);
            if (user == null)
                return NotFound($"User with username {model.Username} not found");

            user.Role = model.Role;
            _usersService.Update(user.Id, user);

            return Ok(new { message = "Role assigned successfully" });
        }

        [HttpGet("edit/{username}")]
        public IActionResult Edit(string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            var mappedUser = _modelMapper.Map(user);
            return Ok(mappedUser);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditUser([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            if (user == null)
                return NotFound("User not found");

            if (_usersService.UserEmailExists(model.Email) && user.Email != model.Email)
                return BadRequest("User with this email already exists.");

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            if (model.UploadImage != null)
            {
                var result = await _photoService.UploadImageAsync(model.UploadImage);
                if (result.Error != null)
                    return BadRequest("An error occurred while uploading the image");

                user.Image = result.Url.ToString();
            }

            var userToEdit = _modelMapper.MapUserViewModel(model);
            _usersService.Update(user.Id, userToEdit);

            return Ok(new { message = "User updated successfully" });
        }

        [HttpPost("send-money")]
        public async Task<IActionResult> SendMoney([FromBody] SendMoneyViewModel sendMoney)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            if (user == null)
                return NotFound("User not found");

            var wallet = user.UserWallets.FirstOrDefault(x => x.Currency == sendMoney.Currency);

            var recipient = _usersService.FindRecipient(new UserQueryParameters
            {
                Username = sendMoney.RecipienTokens,
                PhoneNumber = sendMoney.RecipienTokens,
                Email = sendMoney.RecipienTokens
            });

            if (recipient == null)
                return NotFound($"Recipient with credentials {sendMoney.RecipienTokens} does not exist.");

            var recipientWallet = recipient.UserWallets.FirstOrDefault(x => x.Currency == sendMoney.Currency);
            if (recipientWallet == null)
            {
                recipientWallet = new Wallet
                {
                    Currency = sendMoney.Currency,
                    Amount = 0,
                    Owner = recipient,
                    WalletName = $"{sendMoney.Currency.ToString()} wallet"
                };
                recipient.UserWallets.Add(recipientWallet);
            }

            if (sendMoney.Amount >= 300)
            {
                await _walletService.SendConfirmationEmailAsync(user);
                return Ok(new { message = "Large transaction verification required" });
            }

            _walletService.TransferFunds(sendMoney.Amount, sendMoney.Currency, wallet, recipientWallet, user);
            return Ok(new { message = "Transaction successful" });
        }

        [HttpGet("user-saving-wallets")]
        public IActionResult UserSavingWallets()
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            return Ok(user.SavingWallets);
        }

        [HttpGet("list-transactions")]
        public async Task<IActionResult> ListTransactions([FromQuery] int currentPageIndex)
        {
            var transactions = await GetTransactionsList(currentPageIndex);
            return Ok(transactions);
        }

        [HttpGet("list-users")]
        public async Task<IActionResult> ListUsers([FromQuery] int currentPageIndex)
        {
            var users = await GetUserList(currentPageIndex);
            return Ok(users);
        }

        [HttpPost("search-transaction-by-sender")]
        public IActionResult SearchTransactionBySender([FromBody] string text)
        {
            var transactions = _transactionService.FilterBy(new TransactionQueryParameters { Sender = text })
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("search-transaction-by-recipient")]
        public IActionResult SearchTransactionByRecipient([FromBody] string text)
        {
            var transactions = _transactionService.FilterBy(new TransactionQueryParameters { Recipient = text })
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("search-transaction-by-type")]
        public IActionResult SearchTransactionByType([FromBody] string text)
        {
            if (text == "-")
                return RedirectToAction("ListTransactions");

            var transactions = _transactionService.FilterBy(new TransactionQueryParameters { TransactionType = text })
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("sort-by-date")]
        public IActionResult SortByDate([FromBody] string text)
        {
            var transactions = _transactionService.SortByDate(text)
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("sort-by-amount")]
        public IActionResult SortByAmount([FromBody] string text)
        {
            var transactions = _transactionService.SortByAmount(text)
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("get-date-to-date")]
        public IActionResult GetDateToDate([FromBody] DateTime startDate, DateTime endDate)
        {
            var transactions = _transactionService.GetTransactionsByDateRange(startDate, endDate)
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(transactions);
        }

        [HttpPost("search-by-username")]
        public IActionResult SearchByUsername([FromBody] string username)
        {
            var users = _usersService.FilterBy(new UserQueryParameters { Username = username })
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(users);
        }

        [HttpPost("search-by-email")]
        public IActionResult SearchByEmail([FromBody] string email)
        {
            var users = _usersService.FilterBy(new UserQueryParameters { Email = email })
                .Select(x => _modelMapper.Map(x))
                .ToList();

            return Ok(users);
        }

        [HttpPost("block-user")]
        public IActionResult BlockUser([FromBody] string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            user.IsBlocked = true;
            _usersService.Update(user.Id, user);

            return Ok(new { message = "User blocked successfully" });
        }

        [HttpPost("unblock-user")]
        public IActionResult UnblockUser([FromBody] string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            user.IsBlocked = false;
            _usersService.Update(user.Id, user);

            return Ok(new { message = "User unblocked successfully" });
        }

        [HttpPost("delete-user")]
        public IActionResult DeleteUser([FromBody] string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
                return NotFound("User not found");

            _usersService.Delete(user.Id, user);

            return Ok(new { message = "User deleted successfully" });
        }
		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>() //delete some rows for the claims.
            {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role , user.Role.ToString())

			};


			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
		private async Task<UserPViewModel> GetUserList(int currentPage)
		{
			int maxRowsPerPage = 2;
			UserPViewModel userModel = new UserPViewModel();

			userModel.UserList = await _usersService.GetAll()
				.OrderBy(x => x.Id)
				.Skip((currentPage - 1) * maxRowsPerPage)
				.Take(maxRowsPerPage)
				.ToListAsync();

			double pageCount = (double)((decimal)_usersService.GetAll().Count() / Convert.ToDecimal(maxRowsPerPage));

			userModel.pageCount = (int)Math.Ceiling(pageCount);
			userModel.currentPageIndex = currentPage;
			return userModel;
		}



		private async Task<ListTransactionsViewModel> GetTransactionsList(int currentPage)
		{
			int maxRowsPerPage = 2;
			ListTransactionsViewModel transactionModel = new ListTransactionsViewModel();

			transactionModel.TransactionsList = await _transactionService.GetAllTransactions()
				.OrderBy(x => x.Id)
				.Skip((currentPage - 1) * maxRowsPerPage)
				.Take(maxRowsPerPage)
				.ToListAsync();

			double pageCount = (double)((decimal)_transactionService.GetAllTransactions().Count() / Convert.ToDecimal(maxRowsPerPage));

			transactionModel.pageCount = (int)Math.Ceiling(pageCount);
			transactionModel.currentPageIndex = currentPage;
			return transactionModel;
		}
	}
}