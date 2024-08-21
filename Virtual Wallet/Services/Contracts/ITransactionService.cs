using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface ITransactionService
    {
        Task CreateTransaction(DateTime timestamp, Currency currency, decimal amount, TransactionType type, int walletId);
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<Transaction> GetTransactionById(int id);
        Task<IEnumerable<Transaction>> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetTransactionsByType(TransactionType type);
        Task<IEnumerable<Transaction>> GetTransactionsByWalletId(int walletId);
    }
}