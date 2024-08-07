using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Helpers.Contracts;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Service.Contracts;

namespace Virtual_Wallet.Controllers.MVC
{
    public class UserController : Controller
    {
        private readonly IUserService _usersService;
        private readonly IConfiguration _configuration;
        private readonly IPhotoService _photoService;
        private readonly IModelMapper _modelMapper;

        public UserController(IUserService usersService, IConfiguration configuration, IPhotoService photoService, IModelMapper modelMapper)
        {
            _usersService = usersService;
            _configuration = configuration;
            _photoService = photoService;
            _modelMapper = modelMapper;
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

                CreatePasswordHash(registerModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User
                {
                    Email = registerModel.Email,
                    Username = registerModel.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                //if (registerModel.Image != null)
                //{
                //    var result = await _photoService.AddPhotoAsync(registerModel.Image);

                //    user.Image = result.Url.ToString();
                //}

                User createdUser = _usersService.Create(user);
                UserResponseDTO responseDTO = _modelMapper.MapUser(createdUser);

                string token = CreateToken(user);
                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Change to true in production
                    SameSite = SameSiteMode.Strict
                });

                // Optionally, you can log the user in here or redirect to a login page
                return RedirectToAction("Index", "Home");
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
                //Image = user.Image,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>() //delete some rows for the claims.
            {
                new Claim(ClaimTypes.Name, user.Username),
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
    }
}
