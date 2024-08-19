using Microsoft.EntityFrameworkCore;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool BlockUser(int userId, User user)
        {
            return _userRepository.BlockUser(userId, user);
        }

        public User Create(User user)
        {
            if (_userRepository.UserEmailExists(user.Email))
            {
                throw new DuplicateEntityException($"A user with e-mail: {user.Email} already exists !");
            }

            User createdUser = _userRepository.Create(user);
            return createdUser;

        }

        public bool Delete(int id, User user)
        {
            User userToDelete = _userRepository.GetById(id);

            if (userToDelete.Id != user.Id && user.IsAdmin == false)
            {
                throw new NotAuthorizedException("You are not authorised to delete. Only admin or the owner of this user can delete!");
            }

            return _userRepository.Delete(id);
        }

        public IQueryable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }
        public bool UserEmailExists(string email)
        {
            return GetAll().Any(u => u.Email == email);
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public bool UnblockUser(int userId, User user)
        {
            return _userRepository.UnblockUser(userId, user);
        }

        public User Update(int id, User userUpdate /*User user*/)
        {
            //User userToUpdate = this._usersRepository.GetById(id);

            //if (userToUpdate.Id != user.Id && user.IsAdmin == false)
            //{
            //    throw new NotAuthorizedException("You are not authorised to edit. Only admin or the owner of the user can edit!");
            //}

            User updatedUser = _userRepository.Update(id, userUpdate);
            return updatedUser;
        }

        public void AddUserCard(Card card, User user)
        {
            _userRepository.AddUserCard(card, user);
        }
        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            return _userRepository.FilterBy(filterParameters);
        }

    }
}
