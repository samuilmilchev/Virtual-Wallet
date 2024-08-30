using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.DTOs.UserDTOs
{
    public class CombinedUserDTO
    {
        public RegisterViewModel Register { get; set; }
        public UserDTO? Login { get; set; }
        public WalletDTO Wallet { get; set; }
    }
}
