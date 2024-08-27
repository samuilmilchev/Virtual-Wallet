using Virtual_Wallet.DTOs.TransactionDTOs;
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

		public async Task CreateTransaction(DateTime timestamp, Currency currency, decimal amount, TransactionType type, int walletId)
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

		public async Task<Transaction> GetTransactionById(int id)
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

		public IQueryable<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
		{
			return _transactionRepository.GetByDateRange(startDate, endDate);
		}

		public IQueryable<Transaction> GetAllTransactions()
		{
			return  _transactionRepository.GetAll();
		}

		public List<Transaction> FilterBy(TransactionQueryParameters transactionQueryParameters)
		{
			return _transactionRepository.FilterBy(transactionQueryParameters);
		}

		public List<Transaction> SortByDate(string text)
		{
			return _transactionRepository.SortByDate(text);
		}
        public IQueryable<Transaction> SortByAmount(string text)
        {
            return _transactionRepository.SortByAmount(text);
        }
    }
}
