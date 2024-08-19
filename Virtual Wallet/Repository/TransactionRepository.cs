using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Virtual_Wallet.Db;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

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

		// Get a transaction by its ID
		public async Task<Models.Entities.Transaction> GetById(Guid id)
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
		public async Task<IEnumerable<Models.Entities.Transaction>> GetByDateRange(DateTime startDate, DateTime endDate)
		{
			return await _context.Transactions
				.Where(t => t.Timestamp >= startDate && t.Timestamp <= endDate)
				.Include(t => t.Wallet)
				.ToListAsync();
		}

		// Get all transactions
		public async Task<IEnumerable<Models.Entities.Transaction>> GetAll()
		{
			return await _context.Transactions
				.Include(t => t.Wallet)
				.ToListAsync();
		}
	}
}
