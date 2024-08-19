using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Virtual_Wallet.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string WalletName { get; set; }
		public string BalancesJson { get; set; }

		// This property will be used to interact with the balances as a dictionary
		[NotMapped]
		public Dictionary<string, decimal> Balances
		{
			get => BalancesJson == null ? new Dictionary<string, decimal>() : JsonSerializer.Deserialize<Dictionary<string, decimal>>(BalancesJson);
			set => BalancesJson = JsonSerializer.Serialize(value);
		}

		public string CurrentCurrency { get; set; }

		[Timestamp]
		public byte[] RowVersion { get; set; }  // Concurrency token
	}
}
