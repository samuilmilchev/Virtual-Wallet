using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IWalletRepository
    {
        void AddFunds(decimal amount, Currency currency, Wallet wallet, User user);
        //void ConvertFunds(Wallet wallet, string fromCurrency, string toCurrency, decimal amount);
        Wallet Create(Wallet wallet);
        //decimal GetBalance(Wallet wallet, string currency);
        void WithdrawFunds(decimal amount, Wallet wallet);
		Task<Wallet> GetById(int id);
        void SendMoney(decimal amount, Currency currency, Wallet fromWallet, Wallet toWallet, User user);
        void AddFundsToRecipient(decimal amount, Currency currency, Wallet wallet, User user);
        void CreateSavingWallet(SavingWalletViewModel model);

    }
}