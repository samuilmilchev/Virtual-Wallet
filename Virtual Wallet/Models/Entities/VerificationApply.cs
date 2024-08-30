namespace Virtual_Wallet.Models.Entities
{
    public class VerificationApply
    {
        public int Id { get; set; }
        public string Selfie { get; set; }
        public string IdPhoto { get; set; }
        public User User { get; set; }
    }
}
