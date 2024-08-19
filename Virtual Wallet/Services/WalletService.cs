using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
	public class WalletService : IWalletService
	{
		private readonly IWalletRepository walletRepository;
		private readonly ICardService cardService;
		private readonly IExchangeRateService exchangeRateService; // Assuming you have this service for currency conversion

		public WalletService(IWalletRepository walletRepository, ICardService cardService, IExchangeRateService exchangeRateService)
		{
			this.walletRepository = walletRepository;
			this.cardService = cardService;
			this.exchangeRateService = exchangeRateService;
		}

		public void AddFunds(decimal amount, string currency, Wallet wallet, Card card) // From card to wallet
		{
			if (card.Balance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
			}
			card.Balance -= amount;
			this.walletRepository.AddFunds(amount, currency, wallet);
		}

		public decimal ConvertFunds(decimal amount, string fromCurrency, string toCurrency, Wallet wallet)
		{
			if (fromCurrency == toCurrency)
			{
				throw new InvalidOperationException("Cannot convert between the same currency.");
			}

			var currentBalance = walletRepository.GetBalance(wallet, fromCurrency);
			if (currentBalance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds in the source currency to execute the conversion.");
			}

			var exchangeRate = exchangeRateService.GetExchangeRate(fromCurrency, toCurrency);
			if (exchangeRate <= 0)
			{
				throw new InvalidOperationException("Invalid exchange rate retrieved.");
			}

			var convertedAmount = amount * exchangeRate;

			walletRepository.WithdrawFunds(amount, fromCurrency, wallet);
			walletRepository.AddFunds(convertedAmount, toCurrency, wallet);

			return convertedAmount;
		}

		public Wallet Create(Wallet wallet)
		{
			return walletRepository.Create(wallet);
		}

		public decimal GetBalance(Wallet wallet, string currency)
		{
			return this.walletRepository.GetBalance(wallet, currency);
		}

		public void WithdrawFunds(decimal amount, string currency, Wallet wallet, Card card) // From wallet to card
		{
			var walletBalance = this.walletRepository.GetBalance(wallet, currency);
			if (walletBalance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
			}

			this.walletRepository.WithdrawFunds(amount, currency, wallet);
			card.Balance += amount;
		}

		// New method: Transfer funds from one wallet to another
		public void TransferFunds(decimal amount, string currency, Wallet fromWallet, Wallet toWallet)
		{
			var fromWalletBalance = this.walletRepository.GetBalance(fromWallet, currency);
			if (fromWalletBalance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
			}

			this.walletRepository.WithdrawFunds(amount, currency, fromWallet);
			this.walletRepository.AddFunds(amount, currency, toWallet);
		}

		// New method: Withdraw funds from wallet to cash
		public void WithdrawToCash(decimal amount, string currency, Wallet wallet)
		{
			var walletBalance = this.walletRepository.GetBalance(wallet, currency);
			if (walletBalance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the withdrawal!");
			}

			this.walletRepository.WithdrawFunds(amount, currency, wallet);
			// Handle the cash withdrawal externally (e.g., by interfacing with a cash-handling service)
		}
	}
}