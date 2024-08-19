using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IWalletRepository
    {
        void AddFunds(decimal amount, string currency, Wallet wallet);
        //void ConvertFunds(Wallet wallet, string fromCurrency, string toCurrency, decimal amount);
        Wallet Create(Wallet wallet);
        decimal GetBalance(Wallet wallet, string currency);
        void WithdrawFunds(decimal amount, string currency, Wallet wallet);
		Task<Wallet> GetById(int id);
	}
}