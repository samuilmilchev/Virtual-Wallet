namespace Virtual_Wallet.Models.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationData { get; set; }
        public string CardHolder { get; set; }
        public string CheckNumber { get; set; }
        public CardType CardType { get; set; }
        public decimal Balance { get; set; } = 100000m;

        // Add this line to create a relationship to the User
        public int UserId { get; set; } // Foreign key to User
        public User User { get; set; }  // Navigation property back to User
    }
}
