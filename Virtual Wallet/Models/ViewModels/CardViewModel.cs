using System.ComponentModel.DataAnnotations;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class CardViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username is required and cannot be empty.")]
        public string CardHolder { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Card number is required and must not be empty.")]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "Invalid card number format! Must be XXXX XXXX XXXX XXXX.")]
        public string CardNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Expiration date is required and must not be empty.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Invalid expiration date format! Must be MM/YY.")]
        public string ExpirationDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CVV is required and must not be empty.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVV format! Must be XXX.")]
        public string CheckNumber { get; set; }
        public CardType CardType { get; set; }
    }
}
