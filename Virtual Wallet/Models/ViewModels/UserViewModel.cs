using System.ComponentModel.DataAnnotations;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class UserViewModel
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be empty.")]
        //[StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        //public string FirstName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be empty.")]
        //[StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        //public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required and cannot be empty.")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required and must not be empty.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; }
        public List<Card> Cards { get; set; }
        public bool IsBlocked { get; set; }
        public string? Image { get; set; } //въпрос към Сами: Защо са две: Image и UploadImage
        public IFormFile? UploadImage { get; set; }
        //public string? Selfie { get; set; }
        //public string? IdPhoto { get; set; }
        public bool AdminVerified { get; set; }
        public bool IsAdmin { get; set; }
        public List<Wallet> Wallets { get; set; }
    }
}
