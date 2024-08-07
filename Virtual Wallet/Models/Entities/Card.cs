namespace Virtual_Wallet.Models.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationData { get; set; }
        public int CardHolderId { get; set; }
        public string CardHolder { get; set; }
        public int CheckNumber {  get; set; }
        public CardType CardType { get; set; }
    }
}
