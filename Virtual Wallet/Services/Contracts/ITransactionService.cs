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

        Task<IEnumerable<Transaction>> GetTransactionByUserId(int userId);


        IQueryable<Transaction> UserGetTransactionsByDateRange(DateTime startDate, DateTime endDate, int userId);
       
        List<Transaction> UserFilterBy(TransactionQueryParameters transactionQueryParameters, int userId);
        List<Transaction> UserSortByDate(string text, int userId);
        IQueryable<Transaction> UserSortByAmount(string text, int userId);
    }
}