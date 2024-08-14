using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IWalletService
    {
        Wallet Create(Wallet wallet);
        void AddFunds(double amount, Wallet wallet, Card card);
        void WithdrawFunds(double amount, Wallet wallet, Card card);
        double GetBalance(Wallet wallet);
        double ConvertFunds();
    }
}
