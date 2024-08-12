namespace Virtual_Wallet.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string WalletName { get; set; }

        public double Balance { get; set; }
    }
}
