using Virtual_Wallet.Db;
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

        public void Create()
        {
            throw new NotImplementedException();
        }
    }
}
