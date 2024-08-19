using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;
using Virtual_Wallet.Db;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Repository
{
    public class WalletRepository : IWalletRepository
	{
		private readonly ApplicationContext _context;

		public WalletRepository(ApplicationContext context)
		{
			_context = context;
		}

		public void AddFunds(decimal amount, string currency, Wallet wallet)
		{
			if (amount <= 0)
			{
				throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
			}

			var currentWallet = _context.Wallets.Include(w => w.Owner).FirstOrDefault(w => w.Id == wallet.Id);
			if (currentWallet == null)
			{
				throw new ArgumentException("Wallet not found", nameof(wallet));
			}

			if (currentWallet.Balances == null)
			{
				currentWallet.Balances = new Dictionary<string, decimal>();
			}

			if (!currentWallet.Balances.ContainsKey(currency))
			{
				currentWallet.Balances[currency] = 0;
			}

			currentWallet.Balances[currency] += amount;

			var transaction = new Transaction(DateTime.Now, currency, amount, TransactionType.Add, currentWallet.Id);
			_context.Transactions.Add(transaction);

			_context.SaveChanges();
		}

		public void WithdrawFunds(decimal amount, string currency, Wallet wallet)
		{
			if (amount <= 0)
			{
				throw new ArgumentException("Amount to withdraw must be greater than zero", nameof(amount));
			}

			var currentWallet = _context.Wallets.Include(w => w.Owner).FirstOrDefault(w => w.Id == wallet.Id);
			if (currentWallet == null)
			{
				throw new ArgumentException("Wallet not found", nameof(wallet));
			}

			if (currentWallet.Balances == null || !currentWallet.Balances.ContainsKey(currency) || currentWallet.Balances[currency] < (decimal)amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the withdrawal!");
			}

			currentWallet.Balances[currency] -= (decimal)amount;

			var transaction = new Transaction(DateTime.Now, currency, amount, TransactionType.Withdraw, currentWallet.Id);
			_context.Transactions.Add(transaction);

			_context.SaveChanges();
		}

		public Wallet Create(Wallet wallet)
		{
			if (wallet.Balances == null)
			{
				wallet.Balances = new Dictionary<string, decimal>();
			}

			_context.Wallets.Add(wallet);
			_context.SaveChanges();

			return wallet;
		}

		public decimal GetBalance(Wallet wallet, string currency)
		{
			if (wallet.Balances == null || !wallet.Balances.ContainsKey(currency))
			{
				return 0;
			}

			return wallet.Balances[currency];
		}

		public void ConvertFunds(Wallet wallet, string fromCurrency, string toCurrency, decimal amount)
		{
			if (amount <= 0)
			{
				throw new ArgumentException("Amount to convert must be greater than zero", nameof(amount));
			}

			if (wallet.Balances == null || !wallet.Balances.ContainsKey(fromCurrency) || wallet.Balances[fromCurrency] < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the conversion!");
			}

			// Get the conversion rate
			if (!ConversionRates.TryGetValue((fromCurrency, toCurrency), out var conversionRate)) // Conversion rates should be taken from ApplicationContext??
			{
				throw new InvalidOperationException($"Conversion rate from {fromCurrency} to {toCurrency} is not available.");
			}

			// Convert the amount
			var convertedAmount = amount * conversionRate;

			// Subtract the amount from the original currency balance
			wallet.Balances[fromCurrency] -= amount;

			// Add the converted amount to the target currency balance
			if (!wallet.Balances.ContainsKey(toCurrency))
			{
				wallet.Balances[toCurrency] = 0;
			}
			wallet.Balances[toCurrency] += convertedAmount;

			// Log the conversion transaction
			var transaction = new Transaction(DateTime.Now, toCurrency, convertedAmount, TransactionType.Convert, wallet.Id);
			_context.Transactions.Add(transaction);

			_context.SaveChanges();
		}
		public async Task<Wallet> GetById(int id)
		{
			return await _context.Wallets
				.FirstOrDefaultAsync(w => w.Id == id);
		}
	}
}
