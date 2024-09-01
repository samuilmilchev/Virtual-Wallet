using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IWalletService
    {
        Wallet Create(Wallet wallet);
        void AddFunds(decimal amount, Currency currency, Wallet wallet, Card card, User user);
        void WithdrawFunds(decimal amount, Wallet wallet, Card card);
        void TransferFunds(decimal amount, Currency currency, Wallet fromWallet, Wallet toWallet, User user);
        //decimal GetBalance(Wallet wallet, string currency);
        void ConvertFunds(decimal amount, Currency fromCurrency, Currency toCurrency, string username);
        Wallet GetByCurrency(Currency currency, User user);
        void CreateSavingWallet(SavingWalletViewModel model);
        decimal CalculateInterest(SavingWalletViewModel model);
        decimal CalculateTotal(SavingWalletViewModel model);
    }
}
