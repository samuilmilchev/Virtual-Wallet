namespace Virtual_Wallet.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string WalletName { get; set; }

        public decimal Balance { get; set; } = 0;

        public List<Transaction> TransactionHistory { get; set; } = new List<Transaction>();//инициализиране на списъка с трансакции за да се избегне null reference;

        //public Wallet() //инициализиране на списъка с трансакции в конструктора за да се избегне null reference
        //{
        //    TransactionHistory = new List<Transaction>(); // Initialize the transaction list
        //}
    }
}
