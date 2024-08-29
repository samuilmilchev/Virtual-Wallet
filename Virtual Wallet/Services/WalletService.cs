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
		private readonly Currencyapi currencyapi;
		//private readonly IExchangeRateService exchangeRateService; // Assuming you have this service for currency conversion

		public WalletService(IWalletRepository walletRepository, ICardService cardService/*, IExchangeRateService exchangeRateService*/)
		{
			this.walletRepository = walletRepository;
			this.cardService = cardService;
			//this.exchangeRateService = exchangeRateService;
		}

		public void AddFunds(decimal amount, Currency currency, Wallet wallet, Card card, User user) // From card to wallet
		{
			if (card.Balance < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
			}
			card.Balance -= amount;
			this.walletRepository.AddFunds(amount, currency, wallet, user);
		}

		public void ConvertFunds(decimal amount, Currency fromCurrency, Currency toCurrency, Wallet fromWallet, Wallet toWallet, User user)
        {
			fromCurrency = Currency.BGN;
			toCurrency = Currency.EUR;

            if (fromCurrency == toCurrency)
            {
                throw new InvalidOperationException("Cannot convert between the same currency.");
            }

            if (fromWallet.Amount < amount)
            {
                throw new InsufficientFundsException("Insufficient funds in the source currency to execute the conversion.");
            }

			string JsonExchangeRateReturn = currencyapi.Latest(fromCurrency, toCurrency);

			string[] strings = JsonExchangeRateReturn.Split(':');

            //decimal exchangeRate = currencyapi.Latest(fromCurrency, toCurrency);

            //var convertedAmount = amount * exchangeRate;

            //walletRepository.WithdrawFunds(amount, fromWallet);
            //walletRepository.AddFunds(convertedAmount, toCurrency, toWallet, user);
        }

        public Wallet Create(Wallet wallet)
		{
			return walletRepository.Create(wallet);
		}

		//public decimal GetBalance(Wallet wallet, Currency currency)
		//{
		//	return this.walletRepository.GetBalance(wallet, currency);
		//}

		public void WithdrawFunds(decimal amount, Wallet wallet, Card card) // From wallet to card
		{
            if (wallet.Amount < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
			}

			this.walletRepository.WithdrawFunds(amount, wallet);
			card.Balance += amount;
			this.cardService.UpdateCardBalance(card.Id, card);
		}

		//New method: Transfer funds from one wallet to another
		public void TransferFunds(decimal amount, Currency currency, Wallet fromWallet, Wallet toWallet, User user)
		{
			walletRepository.SendMoney(amount, currency, fromWallet, toWallet, user);
        }

	}
}