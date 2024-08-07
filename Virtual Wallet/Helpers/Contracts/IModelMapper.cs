using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Helpers.Contracts
{
    public interface IModelMapper
    {
        UserResponseDTO MapUser(User user);
    }
}
