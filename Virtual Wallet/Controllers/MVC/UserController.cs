﻿using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Virtual_Wallet.DTOs.TransactionDTOs;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Helpers.Contracts;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Services;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class UserController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly IModelMapper _modelMapper;
        private readonly IWalletService _walletService;
        private readonly IPhotoService _photoService;
        private readonly ITransactionService _transactionService;
        private readonly IEmailService _emailService;

        public UserController(IUsersService usersService, IConfiguration configuration, IModelMapper modelMapper, IWalletService walletService, ITransactionService transactionService, IPhotoService photoService , IEmailService emailService)
        {
            _usersService = usersService;
            _configuration = configuration;
            _modelMapper = modelMapper;
            _walletService = walletService;
            _photoService = photoService;
            _transactionService = transactionService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new CombinedUserDTO
            {
                Login = new UserDTO(),
                Register = new RegisterViewModel()
            };
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(CombinedUserDTO model)
        {
            if (ModelState.IsValid)
            {

                var registerModel = model.Register;
                var walletModel = model.Wallet;

                CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User
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
                    if (result.Error == null)
                    {
                        user.Image = result.Url.ToString();
                    }
                    else
                    {
                        ModelState.AddModelError("ImageUploadError", "An error occurred while uploading the image");
                        return View("Index", model);
                    }
                }

                //при създаване на акаунт се създава и първия(може би и единствен) уолет на юзъра
                Wallet wallet = new Wallet
                {
                    WalletName = walletModel.WalletName,
                    Owner = user,
                    Currency = Currency.BGN
                };

                //тук добавяме новосъздадения уолет към юзъра
                user.UserWallets.Add(wallet);


                //if (registerModel.Image != null)
                //{
                //    var result = await _photoService.AddPhotoAsync(registerModel.Image);

                //    user.Image = result.Url.ToString();
                //}

                User createdUser = _usersService.Create(user);
                //Wallet createdWallet = _walletService.Create(wallet); // не знам дали е нужно за сега 
                UserResponseDTO responseDTO = _modelMapper.MapUser(createdUser);


                string token = CreateToken(user);
                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Change to true in production
                    SameSite = SameSiteMode.Strict
                });

                await _usersService.SendConfirmationEmailAsync(user);

                //return RedirectToAction("Index", "Home");
                // return Ok("Registration successful. Please check your email to verify your account.");
                return View("EmailConfirmationMessage"); //препраща към страница, коята казва на потребителя да си потвърди е-мейла

            }

            return View("Index", model);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(CombinedUserDTO model)
        {
            try
            {
                var loginRequest = model.Login;
                User user = _usersService.GetByUsername(loginRequest.Username);
                if (user == null || !VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                    return BadRequest("Invalid credentials");

                if (user.IsEmailVerified != true) //проверка дали е верифициран мейла
                    return Unauthorized("Email not confirmed. Please check your email to confirm your account."); //Unauthorised, защото други варианти разрушават логиката



                string token = CreateToken(user);
                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Change to true in production
                    SameSite = SameSiteMode.Strict
                });


                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException x)
            {

                ViewData["ErrorMessage"] = x.Message;
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult UserDetails(string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
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
            return View(model);
        }

        [HttpGet]
        public IActionResult UploadPhotosVerification()
        {
            VerifyUserViewModel model = new VerifyUserViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhotosVerification(VerifyUserViewModel model)
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            var selfieResult = await _photoService.UploadImageAsync(model.Selfie);
            user.Selfie = selfieResult.Url.ToString();

            var idPhotoResult = await _photoService.UploadImageAsync(model.IdPhoto);
            user.IdPhoto = idPhotoResult.Url.ToString();

            _usersService.UploadPhotoVerification(selfieResult.Url.ToString(), idPhotoResult.Url.ToString(), user);

            return RedirectToAction("UserDetails", new { username = user.Username });
        }

        [HttpGet]
        public IActionResult VerifyUsers()
        {
            var verifications = _usersService.GetAllVereficationApplies().Select(x => _modelMapper.Map(x)).ToList();

            return View(verifications);
        }

        [HttpPost]
        public IActionResult VerifyUsers(string text)
        {

            string[] tokens = text.Split(',');

            string verificationValue = tokens[0];
            var user = _usersService.GetByUsername(tokens[1]);

            if (verificationValue == "Accept")
            {
                _usersService.UpdateUserVerification(user, verificationValue);

                return RedirectToAction("UserDetails", new { username = user.Username });
            }
            else
            {
                _usersService.UpdateUserVerification(user, verificationValue);

                return RedirectToAction("UserDetails", new { username = user.Username });
            }
        }

        [HttpGet]
        public IActionResult AssignRole()
        {
            return View(new AssignRoleViewModel());
        }


        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult AssignRole(AssignRoleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _usersService.GetByUsername(model.Username);

                if (user == null)
                {
                    ViewData["ErrorMessage"] = $"User with username {model.Username} not found";
                    return View(model);
                }

                user.Role = model.Role;

                var userRole = user.Role;
                _usersService.Update(user.Id, user);

                ViewData["Message"] = "Role assigned successfully";
                return View(new AssignRoleViewModel());
            }
            catch (EntityNotFoundException x)
            {

                ViewData["ErrorMessage"] = x.Message;
                return View(model);
            }

        }
        [HttpGet]
        public IActionResult Edit(string username)
        {
            var user = _usersService.GetByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            var mappedUser = _modelMapper.Map(user);

            // Return the view directly with the mapped user
            return View("EditUser", mappedUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Identity.Name;
                    var user = _usersService.GetByUsername(username);

                    if (user != null)
                    {
                        if (_usersService.UserEmailExists(model.Email) && user.Email != model.Email)
                        {
                            throw new DuplicateEntityException("User with this email already exists.");
                        }
                        user.Email = model.Email;
                        user.PhoneNumber = model.PhoneNumber;
                    }
                    if (model.UploadImage != null)
                    {
                        var result = await _photoService.UploadImageAsync(model.UploadImage);

                        user.Image = result.Url.ToString();
                    }

                    var userToEdit = _modelMapper.MapUserViewModel(model);
                    var editedUser = _usersService.Update(user.Id, userToEdit);
                    return RedirectToAction("UserDetails", new { username = editedUser.Username });
                }
                return View(model);
            }
            catch (DuplicateEntityException x)
            {
                return Json(new { success = false, message = x.Message });
            }
        }

        [HttpGet]
        public IActionResult SendMoney()
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            SendMoneyViewModel model = new SendMoneyViewModel();
            model.CurrentUser = user;

            return View(model);
        }

        [HttpPost]
        public IActionResult SendMoney(SendMoneyViewModel sendMoney)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(sendMoney);
                }

                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);
                //ViewData["CurrentUser"] = user;


                var wallet = user.UserWallets.FirstOrDefault(x => x.Currency == sendMoney.Currency);

                UserQueryParameters userQueryParameters = new UserQueryParameters();
                userQueryParameters.Username = sendMoney.RecipienTokens;
                userQueryParameters.PhoneNumber = sendMoney.RecipienTokens;
                userQueryParameters.Email = sendMoney.RecipienTokens;

                var recipient = _usersService.FindRecipient(userQueryParameters);

                if (recipient == null)
                {
                    throw new EntityNotFoundException($"Recipien with credentials {sendMoney.RecipienTokens} does not exist.");
                }

                var recipientWallet = recipient.UserWallets.FirstOrDefault(x => x.Currency == sendMoney.Currency);
                if (recipientWallet == null)
                {
                    Wallet newWallet = new Wallet();
                    newWallet.Currency = sendMoney.Currency;
                    newWallet.Amount = 0;
                    newWallet.Owner = user;
                    newWallet.OwnerId = user.Id;
                    newWallet.WalletName = $"{sendMoney.Currency.ToString()} wallet";

                    recipient.UserWallets.Add(newWallet);
                }
                var createdWallet = recipient.UserWallets.FirstOrDefault(x => x.Currency == sendMoney.Currency);

                if (sendMoney.Amount >= 300)
                {
                    _walletService.SendConfirmationEmailAsync(user);

                    TempData["Amount"] = sendMoney.Amount;
                    TempData["Currency"] = sendMoney.Currency;
                    //TempData["SenderWalletCurrency"] = wallet.Currency;
                    //TempData["RecipientWalletCurrency"] = createdWallet.Currency;
                    TempData["SenderUsername"] = user.Username;
                    TempData["RecipientUsername"] = recipient.Username;

                    return RedirectToAction("LargeTransactionVerificationForm", "Wallet");
                }

                

                this._walletService.TransferFunds(sendMoney.Amount, sendMoney.Currency, wallet, createdWallet, user);

                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException x)
            {
                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);
                SendMoneyViewModel model = new SendMoneyViewModel();
                model.CurrentUser = user;

                ViewData["ErrorMessage"] = x.Message;
                return View(model);

                // Json(new { success = false, message = x.Message });
            }
        }

        [HttpGet]
        public IActionResult UserSavingWallets()
        {
            var username = User.Identity.Name;
            var user = _usersService.GetByUsername(username);

            return View(user.SavingWallets);
        }

        [HttpGet]
        public async Task<IActionResult> ListTransactions()
        {
            return View(await GetTransactionsList(1));
        }

        [HttpPost]
        public async Task<IActionResult> ListTransactions([FromForm] int currentPageIndex)
        {
            return View(await GetTransactionsList(currentPageIndex));
        }


        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            return View(await GetUserList(1));
        }

        [HttpPost]
        public async Task<IActionResult> ListUsers([FromForm] int currentPageIndex)
        {
            return View(await GetUserList(currentPageIndex));
        }

        //[HttpGet]
        //public IActionResult ListUsers()
        //{
        //    var users = _usersService.GetAll();

        //    if (users == null)
        //    {
        //        return NotFound();
        //    }

        //    List<UserViewModel> usersList = new List<UserViewModel>();

        //    foreach (var user in users)
        //    {
        //        var model = new UserViewModel
        //        {
        //            Username = user.Username,
        //            Email = user.Email,
        //            //Image = user.Image,
        //            PhoneNumber = user.PhoneNumber,
        //            Role = user.Role.ToString(),
        //            IsBlocked = user.IsBlocked
        //        };

        //        usersList.Add(model);
        //    }

        //    return View(usersList);
        //}

        [HttpPost]
        public IActionResult SearchTransactionBySender([FromForm] string text)
        {
            TransactionQueryParameters transactionQueryParameters = new TransactionQueryParameters();
            transactionQueryParameters.Sender = text;

            var transactions = _transactionService.FilterBy(transactionQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult SearchTransactionByRecipient([FromForm] string text)
        {
            TransactionQueryParameters transactionQueryParameters = new TransactionQueryParameters();
            transactionQueryParameters.Recipient = text;

            var transactions = _transactionService.FilterBy(transactionQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult SearchTransactionByType([FromForm] string text)
        {

            if (text == "-")
            {
                return RedirectToAction("ListTransactions");
            }

            TransactionQueryParameters transactionQueryParameters = new TransactionQueryParameters();
            transactionQueryParameters.TransactionType = text;

            var transactions = _transactionService.FilterBy(transactionQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult SortByDate([FromForm] string text)
        {
            var transactions = _transactionService.SortByDate(text).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult SortByAmount([FromForm] string text)
        {
            var transactions = _transactionService.SortByAmount(text).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult GetDateToDate(DateTime startDate, DateTime endDate)
        {
            var transactions = _transactionService.GetTransactionsByDateRange(startDate, endDate).Select(x => _modelMapper.Map(x)).ToList();

            return View(transactions);
        }

        [HttpPost]
        public IActionResult SearchByUsername([FromForm] string text)
        {
            UserQueryParameters userQueryParameters = new UserQueryParameters();
            userQueryParameters.Username = text;

            var users = _usersService.FilterBy(userQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(users);
        }

        [HttpPost]
        public IActionResult SearchByEmail([FromForm] string text)
        {
            UserQueryParameters userQueryParameters = new UserQueryParameters();
            userQueryParameters.Email = text;

            var users = _usersService.FilterBy(userQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(users);
        }

        [HttpPost]
        public IActionResult SearchByNumber([FromForm] string text)
        {
            UserQueryParameters userQueryParameters = new UserQueryParameters();
            userQueryParameters.PhoneNumber = text;

            var users = _usersService.FilterBy(userQueryParameters).Select(x => _modelMapper.Map(x)).ToList();

            return View(users);
        }

        public IActionResult BlockUser(string username)
        {
            var user = _usersService.GetByUsername(username);

            var updatedUser = _usersService.BlockUser(user.Id, user);

            var blockedUser = _modelMapper.Map(user);

            return View("UserDetails", blockedUser);
        }

        public IActionResult UnblockUser(string username)
        {
            var user = _usersService.GetByUsername(username);

            var updatedUser = _usersService.UnblockUser(user.Id, user);

            var blockedUser = _modelMapper.Map(user);

            return View("UserDetails", blockedUser);
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

        private async Task<ListTransactionsViewModel> GetTransactionsList(int currentPage, IQueryable<Transaction> transactions)
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

        [HttpGet]
        public IActionResult ConfirmEmail(string username , string token) //може би измисти ДТО за това
        {
            var user = _usersService.GetByUsername(username);
            ViewData["Username"] = username;

            if (user == null || user.EmailConfirmationToken != token || user.EmailTokenExpiry < DateTime.Now)
            {
                return RedirectToAction("EmailError", new { username = user.Username });
            }

            user.IsEmailVerified = true;
            user.EmailTokenExpiry = null;
            user.EmailConfirmationToken = null;

            _usersService.Update(user.Id , user);

            return View("ConfirmEmail");
        }

        [HttpPost]
        public  async Task<IActionResult> ResendConfirmationEmail(string username)
        {
            var user = _usersService.GetByUsername(username);
            if (user == null)
            {
                return NotFound("User has not been found.");
            }

            await _usersService.SendConfirmationEmailAsync(user);

            TempData["Message"] = "A new confirmation email has been sent to your email address.";
            return RedirectToAction("EmailConfirmationSentAgain");
            
        }

       
    }
}
