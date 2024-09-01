using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class SavingWalletViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public Currency Currency { get; set; }
        public User CurrentUser { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
