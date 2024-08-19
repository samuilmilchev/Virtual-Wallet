using System.ComponentModel.DataAnnotations;

namespace Virtual_Wallet.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string WalletName { get; set; }

		public Dictionary<string, decimal> Balances { get; set; }

		public string CurrentCurrency { get; set; }

		//public List<Transaction> TransactionHistory { get; set; } = new List<Transaction>();//инициализиране на списъка с трансакции за да се избегне null reference;

        [Timestamp]
        public byte[] RowVersion { get; set; }  // Concurrency token , нужен токен за контрол на
                                                // версията на самото ентити(EF Core проверява това проперти за да провери дали
                                                // ентито е имало предишни промени когато извикаме context.SaveChanges())

        //public Wallet() //инициализиране на списъка с трансакции в конструктора за да се избегне null reference
        //{
        //    TransactionHistory = new List<Transaction>(); // Initialize the transaction list
        //}
    }
}
