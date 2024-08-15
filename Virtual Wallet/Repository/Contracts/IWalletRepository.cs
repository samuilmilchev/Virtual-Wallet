using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IWalletRepository
    {
        Wallet Create(Wallet wallet);
        void AddFunds(decimal amount, Wallet wallet);
        void WithdrawFunds(decimal amount, Wallet wallet);
        decimal GetBalance(Wallet wallet);
        decimal ConvertFunds();
    }
}
