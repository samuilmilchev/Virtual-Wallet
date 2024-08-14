
namespace Virtual_Wallet.Models.Entities
{
	public interface IWallet
	{
		Dictionary<string, decimal> Balances { get; }
		string CurrentCurrency { get; }
		Dictionary<string, Dictionary<string, decimal>> ExchangeRates { get; }
		Dictionary<string, string> OwnerInfo { get; }
		List<Transaction> TransactionHistory { get; }
		string WalletId { get; }

		void AddExchangeRate(string baseCurrency, string targetCurrency, decimal rate);
		void AddFunds(decimal amount);
		void AddFunds(string currency, decimal amount);
		void ConvertFunds(string fromCurrency, decimal amount);
		void ConvertFunds(string fromCurrency, string toCurrency, decimal amount);
		decimal GetBalance();
		decimal GetBalance(string currency);
		void SetCurrentCurrency(string currency);
		string ToString();
		void WithdrawFunds(decimal amount);
		void WithdrawFunds(string currency, decimal amount);
	}
}