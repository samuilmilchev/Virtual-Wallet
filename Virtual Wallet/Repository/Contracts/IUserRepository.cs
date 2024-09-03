using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        User GetByUsername(string username);
        User GetByEmail(string email);
        User GetByPhoneNumber (string phoneNumber);
		User Create(User user);
        User Update(int id, User user);
        bool Delete(int id);
        bool BlockUser(int userId, User user);
        bool UnblockUser(int userId, User user);
        User GetById(int id);
        bool UploadPhotoVerification(string selfie, string idPhoto, User user);
        List<VerificationApply> GetAllVereficationApplies();
        void UpdateUserVerification(User user, string text);
        void AddUserCard(Card card, User user);
        bool UserEmailExists(string email);
        bool UserNameExists(string name);
        bool UserPhoneNumberExists(string password);
        List<User> FilterBy(UserQueryParameters filterParameters);
        User FindRecipient(UserQueryParameters recipientDTO);
        void AddFriend(int userId, int friendId);
        void RemoveFriend(int userId, int friendId);
        List<User> GetFriends(int userId);
    }
}
