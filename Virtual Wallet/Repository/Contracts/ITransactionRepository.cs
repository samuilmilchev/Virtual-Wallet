using Virtual_Wallet.DTOs.TransactionDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
        IQueryable<Transaction> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<Transaction> GetById(int id);
        Task<IEnumerable<Transaction>> GetByType(TransactionType type);
        Task<IEnumerable<Transaction>> GetByWalletId(int walletId);
        IQueryable<Transaction> GetAll();
        List<Transaction> FilterBy(TransactionQueryParameters transactionParameters);
        List<Transaction> SortByDate(string text);
        IQueryable<Transaction> SortByAmount(string text);
    }
}