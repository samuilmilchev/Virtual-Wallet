using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.DTOs.TransactionDTOs
{
    public class TransactionQueryParameters
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string  TransactionType { get; set; }
    }
}
