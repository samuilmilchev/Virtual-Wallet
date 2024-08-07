using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetByFirstname(string firstname);
        User Create(User user);
        User Update(int id, User user);
        bool Delete(int id);
        bool BlockUser(int userId, User user);
        bool UnblockUser(int userId, User user);
        User GetById(int id);
        bool UserEmailExists(string email);
        List<User> FilterBy(UserQueryParameters filterParameters);
    }
}
