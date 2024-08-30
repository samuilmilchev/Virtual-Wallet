using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
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

        public void AddFunds(decimal amount, Currency currency, Wallet wallet, User user)
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

			var currencyWallet = user.UserWallets.FirstOrDefault(x => x.Currency == currency);
            if (currencyWallet == null)
            {
				Wallet newWallet = new Wallet();
				newWallet.Currency = currency;
				newWallet.Amount = amount;
				newWallet.Owner = user;
				newWallet.OwnerId = user.Id;
				newWallet.WalletName = $"{currency.ToString()} wallet";

                user.UserWallets.Add(newWallet);
            }
			else
			{
                currencyWallet.Amount += amount;
            }

            // Create a transaction record
            var transaction = new Transaction(DateTime.Now, currency, amount, TransactionType.Add, currentWallet.Id);
			transaction.Recipient = user;
            _context.Transactions.Add(transaction);

            // Save changes to the database
            _context.SaveChanges();
        }

        public void AddFundsToRecipient(decimal amount, Currency currency, Wallet wallet, User user)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
            }

            wallet.Amount += amount;

            // Create a transaction record
            var transaction = new Transaction(DateTime.Now, currency, amount, TransactionType.Add, wallet.Id);
            transaction.Recipient = wallet.Owner;
            _context.Transactions.Add(transaction);

            // Save changes to the database
            _context.SaveChanges();
        }

        public void WithdrawFunds(decimal amount, Wallet wallet)
		{
			if (amount <= 0)
			{
				throw new ArgumentException("Amount to withdraw must be greater than zero", nameof(amount));
			}

			//var currentWallet = _context.Wallets.Include(w => w.Owner).FirstOrDefault(w => w.Id == wallet.Id);
			//if (currentWallet == null)
			//{
			//	throw new ArgumentException("Wallet not found", nameof(wallet));
			//}

			if (wallet.Amount < amount)
			{
				throw new InsufficientFundsException("Insufficient funds to execute the withdrawal!");
			}

			//if (currentWallet.Balances.ContainsKey(currency))
			//{
			//    var dicitonary = new Dictionary<string, decimal>();
			//    dicitonary.Add(currency, currentWallet.Balances[currency]);
			//    dicitonary[currency] -= amount;
			//    currentWallet.Balances = dicitonary;
			//}

			wallet.Amount -= amount;

            var transaction = new Transaction(DateTime.Now, wallet.Currency, amount, TransactionType.Withdraw, wallet.Id);
			transaction.Sender = wallet.Owner;

            _context.Transactions.Add(transaction);

			_context.SaveChanges();
		}

		public Wallet Create(Wallet wallet)
		{
			//if (wallet.Amount == 0)
			//{
			//	wallet.Balances = new Dictionary<string, decimal>();
			//}

			_context.Wallets.Add(wallet);
			_context.SaveChanges();

			return wallet;
		}

		public void SendMoney(decimal amount, Currency currency, Wallet fromWallet, Wallet toWallet, User user)
		{
            if (fromWallet.Amount < amount)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
            }

            this.WithdrawFunds(amount, fromWallet);
            this.AddFundsToRecipient(amount, currency, toWallet, user);

            _context.SaveChanges();
        }

        public async Task<Wallet> GetById(int id)
		{
			return await _context.Wallets
				.FirstOrDefaultAsync(w => w.Id == id);
		}
	}
}
