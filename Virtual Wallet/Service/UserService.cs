using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Service
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool BlockUser(int userId, User user)
        {
            return _userRepository.BlockUser(userId, user);
        }

        public User Create(User user)  
        {
            if (this._userRepository.UserEmailExists(user.Email))
            {
                throw new DuplicateEntityException($"A user with e-mail: {user.Email} already exists !");
            }

            User createdUser = this._userRepository.Create(user);
            return createdUser;

        }

        public bool Delete(int id, User user)
        {
            User userToDelete = this._userRepository.GetById(id);

            if (userToDelete.Id != user.Id && user.IsAdmin == false)
            {
                throw new NotAuthorizedException("You are not authorised to delete. Only admin or the owner of this user can delete!");
            }

            return this._userRepository.Delete(id);
        }

        public List<User> GetAll()
        {
            return this._userRepository.GetAll();
        }

        public User GetByEmail(string email)
        {
            return this._userRepository.GetByEmail(email);
        }

        public User GetByFirstname(string firstname)
        {
            return _userRepository.GetByFirstname(firstname);
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public bool UnblockUser(int userId, User user)
        {
            return this._userRepository.UnblockUser(userId, user);
        }

        public User Update(int id, User userUpdate /*User user*/)
        {
            //User userToUpdate = this._usersRepository.GetById(id);

            //if (userToUpdate.Id != user.Id && user.IsAdmin == false)
            //{
            //    throw new NotAuthorizedException("You are not authorised to edit. Only admin or the owner of the user can edit!");
            //}

            User updatedUser = this._userRepository.Update(id, userUpdate);
            return updatedUser;
        }

        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            return this._userRepository.FilterBy(filterParameters);
        }
    }
}
