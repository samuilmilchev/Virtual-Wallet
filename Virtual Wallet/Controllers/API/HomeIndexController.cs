using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Controllers.API
{
    [Route("api/home")]
    [ApiController]
    public class HomeApiController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IEmailService _emailService;

        public HomeApiController(IUsersService usersService, IEmailService emailService)
        {
            _usersService = usersService;
            _emailService = emailService;
        }

        // GET: api/home/index
        [HttpGet("index")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;
                var user = _usersService.GetByUsername(username);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(new { message = "User authenticated.", user });
            }

            return Unauthorized(new { message = "User is not authenticated." });
        }

        // GET: api/home/about
        [HttpGet("about")]
        public IActionResult About()
        {
            // Since the original About method just returns a view, we will return a simple message
            return Ok(new { message = "This is the About page information." });
        }
    }
}
