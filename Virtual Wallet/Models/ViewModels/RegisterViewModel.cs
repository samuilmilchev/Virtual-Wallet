using System.ComponentModel.DataAnnotations;

namespace Virtual_Wallet.Models.ViewModels
{
    public class RegisterViewModel
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be empty.")]
        //[StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        //public string Firstname { get; set; }

        ////[Required(AllowEmptyStrings = false, ErrorMessage = "The {0} field is required and must not be an empty string.")]
        //[StringLength(32, MinimumLength = 4, ErrorMessage = "The {0} field must be between {2} and {1} characters long.")]
        //public string Lastname { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required and must not be empty.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required and cannot be empty.")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required and cannot be empty.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Phone number is required and cannot be empty.")]
        //[RegularExpression(@"add regex", ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string PhoneNumber { get; set; }

        public IFormFile? Image { get; set; }
    }
}
