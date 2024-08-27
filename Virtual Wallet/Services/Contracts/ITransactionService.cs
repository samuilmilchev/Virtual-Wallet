using Virtual_Wallet.DTOs.TransactionDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface ITransactionService
    {
        Task CreateTransaction(DateTime timestamp, Currency currency, decimal amount, TransactionType type, int walletId);
        Task<Transaction> GetTransactionById(int id);
        IQueryable<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Transaction>> GetTransactionsByType(TransactionType type);
        Task<IEnumerable<Transaction>> GetTransactionsByWalletId(int walletId);
        IQueryable<Transaction> GetAllTransactions();
        List<Transaction> FilterBy(TransactionQueryParameters transactionQueryParameters);
        List<Transaction> SortByDate(string text);
        IQueryable<Transaction> SortByAmount(string text);
    }
}