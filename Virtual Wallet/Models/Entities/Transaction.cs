namespace Virtual_Wallet.Models.Entities
{
	public class Transaction
	{
		public DateTime Timestamp { get; private set; }
		public string Currency { get; private set; }
		public decimal Amount { get; private set; }
		public TransactionType Type { get; private set; } // "Add" or "Withdraw" or "Convert"

		public Transaction(DateTime timestamp, string currency, decimal amount, TransactionType type)
		{
			Timestamp = timestamp;
			Currency = currency;
			Amount = amount;
			Type = type;
		}

		public override string ToString()
		{
			return $"{Timestamp}: {Type} {Amount} {Currency}";
		}
	}
}
