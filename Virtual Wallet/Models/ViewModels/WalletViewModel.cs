using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class WalletViewModel
    {
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string WalletName { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
