using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface IUsersService
    {
        List<User> GetAll();
        User GetByUsername(string username);
        User GetByEmail(string email);
        bool UserEmailExists(string email);
        User Create(User user);
        User Update(int id, User userUpdate/*, User user*/);
        bool Delete(int id, User user);
        bool BlockUser(int userId, User user);
        bool UnblockUser(int userId, User user);
        List<User> FilterBy(UserQueryParameters filterParameters);
    }
}
