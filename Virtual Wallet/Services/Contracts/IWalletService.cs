using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IWalletService
    {
        Wallet Create(Wallet wallet);
        void AddFunds(decimal amount, string currency, Wallet wallet, Card card);
        void WithdrawFunds(decimal amount, string currency, Wallet wallet, Card card);
        decimal GetBalance(Wallet wallet, string currency);
        decimal ConvertFunds(decimal amount, string fromCurrency, string toCurrency, Wallet wallet);
    }
}
