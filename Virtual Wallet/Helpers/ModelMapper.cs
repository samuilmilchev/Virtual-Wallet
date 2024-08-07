using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Helpers
{
    public class ModelMapper
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
    }
}
