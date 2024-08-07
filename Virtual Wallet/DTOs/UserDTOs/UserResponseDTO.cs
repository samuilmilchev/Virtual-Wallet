namespace Virtual_Wallet.DTOs.UserDTOs
{
    public class UserResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        public string? PhoneNumber { get; set; }
        public string Image { get; set; }
    }
}
