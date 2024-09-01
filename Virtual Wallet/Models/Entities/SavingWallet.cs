using System.Data;

namespace Virtual_Wallet.Models.Entities
{
    public class SavingWallet
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal? InterestRate { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
