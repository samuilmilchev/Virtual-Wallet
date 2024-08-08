using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;

namespace Virtual_Wallet.Helpers.Contracts
{
    public interface IModelMapper
    {
        UserResponseDTO MapUser(User user);
        UserViewModel Map(User user);
        User MapUserViewModel(UserViewModel urd);
    }
}
