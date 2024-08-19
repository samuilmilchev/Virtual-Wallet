using Microsoft.EntityFrameworkCore.Metadata;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Helpers.Contracts;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Helpers
{
    public class ModelMapper : IModelMapper
    {
        private readonly IUserRepository _usersRepository;

        public ModelMapper(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public UserResponseDTO MapUser(User user)
        {
            UserResponseDTO udto = new UserResponseDTO();
            udto.Username = user.Username;
            udto.Email = user.Email;
            udto.IsAdmin = user.IsAdmin;
            udto.IsBlocked = user.IsBlocked;

            return udto;
        }

        public UserViewModel Map(User user)
        {
            UserViewModel newUser = new UserViewModel();

            newUser.Username = user.Username;
            newUser.Email = user.Email;
            newUser.PhoneNumber = user.PhoneNumber;
            newUser.IsBlocked = user.IsBlocked;
            newUser.Cards = user.Cards;

            return newUser;
        }

        public User MapUserViewModel(UserViewModel urd)
        {
            return new User
            {
                Email = urd.Email,
                Username = urd.Username,
                PhoneNumber = urd.PhoneNumber
            };
        }
    }
}
