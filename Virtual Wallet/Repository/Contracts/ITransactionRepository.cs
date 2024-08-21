using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
        Task<IEnumerable<Transaction>> GetAll();
        Task<IEnumerable<Transaction>> GetByDateRange(DateTime startDate, DateTime endDate);
        Task<Transaction> GetById(int id);
        Task<IEnumerable<Transaction>> GetByType(TransactionType type);
        Task<IEnumerable<Transaction>> GetByWalletId(int walletId);
    }
}