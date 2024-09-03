using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Virtual_Wallet.Db;
using Virtual_Wallet.DTOs.TransactionDTOs;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Transaction = Virtual_Wallet.Models.Entities.Transaction;

namespace Virtual_Wallet.Repository
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly ApplicationContext _context;

		public TransactionRepository(ApplicationContext context)
		{
			_context = context;
		}

		// Create a new transaction
		public async Task Create(Models.Entities.Transaction transaction)
		{
			if (transaction == null)
			{
				throw new ArgumentNullException(nameof(transaction));
			}

			// Add the transaction to the database context
			_context.Transactions.Add(transaction);

			// Save the changes to the database
			await _context.SaveChangesAsync();
		}

        public async Task<IEnumerable<Transaction>> GetByUserId(int userId)
        {
            return await _context.Transactions
                .Where(t => t.Sender.Id == userId || t.Recipient.Id == userId)
                .Include(t => t.Wallet)
                .Include(t=>t.Sender)
                .Include(t=>t.Recipient)
                .ToListAsync();
        }

        // Get a transaction by its ID
        public async Task<Models.Entities.Transaction> GetById(int id)
		{
			return await _context.Transactions
				.Include(t => t.Wallet)
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		// Get all transactions for a specific wallet
		public async Task<IEnumerable<Models.Entities.Transaction>> GetByWalletId(int walletId)
		{
			return await _context.Transactions
				.Where(t => t.WalletId == walletId)
				.Include(t => t.Wallet)
				.ToListAsync();
		}

		// Get transactions by type (Add, Withdraw, Convert)
		public async Task<IEnumerable<Models.Entities.Transaction>> GetByType(TransactionType type)
		{
			return await _context.Transactions
				.Where(t => t.Type == type)
				.Include(t => t.Wallet)
				.ToListAsync();
		}

		// Get transactions within a specific date range
		public IQueryable<Transaction> GetByDateRange(DateTime startDate, DateTime endDate)
		{
			return _context.Transactions
				.Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
				.Include(t => t.Wallet);
		}

        

        // Get all transactions
        public IQueryable<Transaction> GetAll()
		{
			return  _context.Transactions.Include(t => t.Wallet);
		}

        public List<Transaction> FilterBy(TransactionQueryParameters transactionParameters)
		{
			IQueryable<Transaction> result = this.GetAll();

			result = FilterBySender(result, transactionParameters.Sender);
			result = FilterByRecipient(result, transactionParameters.Recipient);
			result = FilterByTransactionType(result, transactionParameters.TransactionType);

			return result.ToList();
        }

		public List<Transaction> SortByDate(string text)
		{
			if (text == "Oldest")
			{
                return _context.Transactions.Include(x => x.Wallet).ToList();
            }
			else
			{
                return _context.Transactions.Include(x => x.Wallet).OrderByDescending(x => x.Id).ToList();
            }
        }

        public IQueryable<Transaction> SortByAmount(string text)
        {
            if (text == "High")
            {
                return _context.Transactions.Include(x => x.Wallet).OrderByDescending(x => x.Amount);
            }
            else
            {
                return _context.Transactions.Include(x => x.Wallet).OrderBy(x => x.Amount);
            }
        }

        private static IQueryable<Transaction> FilterBySender(IQueryable<Transaction> transactions, string sender)
        {
            if (!string.IsNullOrEmpty(sender))
            {
                return transactions.Where(u => u.Sender.Username == sender);
            }
            else
            {
                return transactions;
            }
        }

        private static IQueryable<Transaction> FilterByRecipient(IQueryable<Transaction> transactions, string recipient)
        {
            if (!string.IsNullOrEmpty(recipient))
            {
                return transactions.Where(u => u.Recipient.Username == recipient);
            }
            else
            {
                return transactions;
            }
        }

        private static IQueryable<Transaction> FilterByTransactionType(IQueryable<Transaction> transactions, string transactionType)
        {
			if (transactionType == TransactionType.Add.ToString())
			{
                if (!string.IsNullOrEmpty(transactionType))
                {
                    return transactions.Where(u => u.Type == TransactionType.Add);
                }
                else
                {
                    return transactions;
                }
            }
			else if(transactionType == TransactionType.Withdraw.ToString())
			{
                if (!string.IsNullOrEmpty(transactionType))
                {
                    return transactions.Where(u => u.Type == TransactionType.Withdraw);
                }
                else
                {
                    return transactions;
                }
            }
			else
			{
				return transactions;
			}
        }

        //=================================================================================================================================

        public IQueryable<Transaction> UserGetByDateRange(DateTime startDate, DateTime endDate, int userId)
        {
            return _context.Transactions
                .Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate && (t.Sender.Id == userId || t.Recipient.Id == userId))
                .Include(t => t.Wallet);
        }

        public List<Transaction> UserFilterBy(TransactionQueryParameters transactionParameters, int userId)
        {
            IQueryable<Transaction> result = this.GetAll();

            result = UserFilterBySender(result, transactionParameters.Sender, userId);
            result = UserFilterByRecipient(result, transactionParameters.Recipient, userId);
            result = UserFilterByTransactionType(result, transactionParameters.TransactionType,userId);

            return result.ToList();
        }

        public List<Transaction> UserSortByDate(string text, int userId)
        {
            if (text == "Oldest")
            {
                return _context.Transactions.Include(x => x.Wallet).Where(t=> t.Sender.Id == userId || t.Recipient.Id == userId).ToList();
            }
            else
            {
                return _context.Transactions.Include(x => x.Wallet).Where(t => t.Sender.Id == userId || t.Recipient.Id == userId).OrderByDescending(x => x.Id).ToList();
            }
        }

        public IQueryable<Transaction> UserSortByAmount(string text, int userId)
        {
            if (text == "High")
            {
                return _context.Transactions.Include(x => x.Wallet).Where(t => t.Sender.Id == userId || t.Recipient.Id == userId).OrderByDescending(x => x.Amount);
            }
            else
            {
                return _context.Transactions.Include(x => x.Wallet).Where(t => t.Sender.Id == userId || t.Recipient.Id == userId).OrderBy(x => x.Amount);
            }
        }

        private static IQueryable<Transaction> UserFilterBySender(IQueryable<Transaction> transactions, string sender, int userId)
        {
            if (!string.IsNullOrEmpty(sender))
            {
                return transactions.Where(u => u.Sender.Username == sender && (u.Sender.Id == userId || u.Recipient.Id == userId));
            }
            else
            {
                return transactions;
            }
        }

        private static IQueryable<Transaction> UserFilterByRecipient(IQueryable<Transaction> transactions, string recipient, int userId)
        {
            if (!string.IsNullOrEmpty(recipient))
            {
                return transactions.Where(u => u.Recipient.Username == recipient && (u.Sender.Id == userId || u.Recipient.Id == userId));
            }
            else
            {
                return transactions;
            }
        }

        private static IQueryable<Transaction> UserFilterByTransactionType(IQueryable<Transaction> transactions, string transactionType, int userId)
        {
            if (transactionType == TransactionType.Add.ToString())
            {
                if (!string.IsNullOrEmpty(transactionType))
                {
                    return transactions.Where(u => u.Type == TransactionType.Add && (u.Sender.Id == userId || u.Recipient.Id == userId));
                }
                else
                {
                    return transactions;
                }
            }
            else if (transactionType == TransactionType.Withdraw.ToString())
            {
                if (!string.IsNullOrEmpty(transactionType))
                {
                    return transactions.Where(u => u.Type == TransactionType.Withdraw && (u.Sender.Id == userId || u.Recipient.Id == userId));
                }
                else
                {
                    return transactions;
                }
            }
            else
            {
                return transactions;
            }
        }
    }
}
