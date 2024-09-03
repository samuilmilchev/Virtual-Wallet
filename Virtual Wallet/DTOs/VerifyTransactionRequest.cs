namespace Virtual_Wallet.DTOs
{
    public class VerifyTransactionRequest
    {
        public string TransactionToken { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
    }
}
