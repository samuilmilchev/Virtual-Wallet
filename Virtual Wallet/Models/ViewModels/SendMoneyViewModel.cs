using System.Globalization;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class SendMoneyViewModel
    {
        public string RecipienTokens { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public User CurrentUser { get; set; }

    }
}
