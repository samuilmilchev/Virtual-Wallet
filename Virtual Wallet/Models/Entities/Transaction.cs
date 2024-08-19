namespace Virtual_Wallet.Models.Entities
{
	public class Transaction // : ITransaction  - не мисля че ни трябва интерфейс за модела на трансакция
	{
		public Guid Id { get; set; } // специално ИД на трансакцията
		public DateTime Timestamp { get; private set; }
		public string Currency { get; private set; } //Закоментирано засега, докато измислим логиката за конвертиране - Няма да конвертираме, добавяме в currency wallet-a
		public decimal Amount { get; private set; }
		public TransactionType Type { get; private set; } // "Add" or "Withdraw" or "Convert"

        // Foreign key to Wallet
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public Transaction(DateTime timestamp, string currency, decimal amount, TransactionType type, int walletId)
        {
            Id = Guid.NewGuid();
            Timestamp = timestamp;
            Currency = currency;
            Amount = amount;
            Type = type;
            WalletId = walletId;
        }
        //public Transaction(DateTime timestamp, string currency, decimal amount, TransactionType type)
        //{
        //	Id = Guid.NewGuid();
        //	Timestamp = timestamp;
        //	Currency = currency;
        //	Amount = amount;
        //	Type = type;
        //}

        //public override string ToString()
        //{
        //	return $"{Timestamp}: {Type} {Amount} {Currency}";
        //}
    }
}
