using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class TransactionService : ITransactionService
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly IWalletRepository _walletRepository;

		public TransactionService(ITransactionRepository transactionRepository, IWalletRepository walletRepository)
		{
			_transactionRepository = transactionRepository;
			_walletRepository = walletRepository;
		}

		public async Task CreateTransaction(DateTime timestamp, string currency, decimal amount, TransactionType type, int walletId)
		{
			// Validate the wallet exists
			var wallet = await _walletRepository.GetById(walletId);
			if (wallet == null)
			{
				throw new ArgumentException("Invalid wallet ID", nameof(walletId));
			}

			// Create the transaction object
			var transaction = new Transaction(timestamp, currency, amount, type, walletId);

			// Add the transaction via the repository
			await _transactionRepository.Create(transaction);
		}

		public async Task<Transaction> GetTransactionById(Guid id)
		{
			return await _transactionRepository.GetById(id);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsByWalletId(int walletId)
		{
			return await _transactionRepository.GetByWalletId(walletId);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsByType(TransactionType type)
		{
			return await _transactionRepository.GetByType(type);
		}

		public async Task<IEnumerable<Transaction>> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
		{
			return await _transactionRepository.GetByDateRange(startDate, endDate);
		}

		public async Task<IEnumerable<Transaction>> GetAllTransactions()
		{
			return await _transactionRepository.GetAll();
		}
	}
}
