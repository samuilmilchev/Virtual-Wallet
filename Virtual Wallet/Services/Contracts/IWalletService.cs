using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IWalletService
    {
        Wallet Create(Wallet wallet);
        void AddFunds(decimal amount, Wallet wallet, Card card);
        void WithdrawFunds(decimal amount, Wallet wallet, Card card);
        decimal GetBalance(Wallet wallet);
        decimal ConvertFunds();
    }
}
