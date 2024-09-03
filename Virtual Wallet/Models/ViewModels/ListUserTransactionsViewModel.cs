using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class ListUserTransactionsViewModel
    {
        public int currentPageIndex { get; set; }
        public int pageCount { get; set; }
        public List<Transaction> TransactionsList { get; set; }
    }
}
