using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public Wallet Wallet { get; set; }
        public User? Sender { get; set; }
        public User? Recipient { get; set; }
    }
}
