using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.DTOs
{
    public class ReviewSavingWalletRequest
    {
        public string Text { get; set; }
        public SavingWalletViewModel Model { get; set; }
    }
}
