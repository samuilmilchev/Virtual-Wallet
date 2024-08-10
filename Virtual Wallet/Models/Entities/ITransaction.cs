
namespace Virtual_Wallet.Models.Entities
{
	public interface ITransaction
	{
		decimal Amount { get; }
		string Currency { get; }
		DateTime Timestamp { get; }
		TransactionType Type { get; }

		string ToString();
	}
}