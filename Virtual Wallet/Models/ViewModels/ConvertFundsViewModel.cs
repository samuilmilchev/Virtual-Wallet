using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class ConvertFundsViewModel
    {
        //decimal amount, Currency fromCurrency, Currency toCurrency, Wallet fromWallet, Wallet toWallet, User user

        public Currency FromCurrency { get; set; }
        public Currency ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}