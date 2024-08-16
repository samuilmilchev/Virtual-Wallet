namespace Virtual_Wallet.Models.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationData { get; set; }
        public int UserId { get; set; }
        public User CardHolder { get; set; }
        public string CheckNumber {  get; set; }
        public CardType CardType { get; set; }
        public decimal Balance { get; set; } = 100000;
    }
}
