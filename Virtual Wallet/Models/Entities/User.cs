namespace Virtual_Wallet.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        //public string Image { get; set; }
        public List<Card> Card { get; set; } // List?
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; } = false;

        public UserRole Role { get; set; }

        public int WalletId { get; set; }
        public Wallet UserWallet  { get; set; } //може би ще подлежи на промяна, като местоположение

    }
}
