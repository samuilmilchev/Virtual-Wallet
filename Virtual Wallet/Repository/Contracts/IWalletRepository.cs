using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IWalletRepository
    {
        Wallet Create(Wallet wallet);
        void AddFunds(double amount, Wallet wallet);
        void WithdrawFunds(double amount, Wallet wallet);
        double GetBalance(Wallet wallet);
        double ConvertFunds();
    }
}
