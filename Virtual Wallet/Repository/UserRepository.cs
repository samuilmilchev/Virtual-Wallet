using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;
using Virtual_Wallet.Db;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public bool Delete(int id)
        {
            User user = this.GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetByEmail(string email)
        {
            User user = this.GetUsers().Include(y => y.Friends).Include(x => x.Cards).Include(u=>u.UserWallets).FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with e-mail: {email} does not exist!");
            }
            return user;
        }
        public User GetByUsername(string username)
        {
            
            User user = this.GetUsers().Include(y => y.Friends).Include(x => x.Cards).Include(u => u.UserWallets).Include(x => x.SavingWallets).FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with username {username} does not exist!");
            }     //commented because an excception should not be visualised in MVC(in the view)
            return user;
        }

        public User GetByPhoneNumber(string phoneNumber)
        {
           
            User user = this.GetUsers().Include(y => y.Friends).Include(x => x.Cards).Include(u => u.UserWallets).FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with phone number {phoneNumber} does not exist!");
            }     //commented because an excception should not be visualised in MVC(in the view)
            return user;
        }

        public User Update(int id, User user)
        {
            User userToUpdate = this.GetById(id);
            if (userToUpdate == null)
            {
                throw new EntityNotFoundException($"User with id {id} does not exist!");
            }

            user.Email = userToUpdate.Email;
            //user.Password = userToUpdate.Password; //TODO: logika za update na parola na veche lognat user

            _context.SaveChanges();
            return userToUpdate;
        }

        public bool BlockUser(int userId, User user)
        {
            User userToBlock = this.GetById(userId);

            if (userToBlock == null)
            {
                throw new EntityNotFoundException($"User was not found.");
            }

            if (user.IsAdmin != true)
            {
                throw new NotAuthorizedException("Not authorized. Only admins can block users.");
            }

            userToBlock.IsBlocked = true;
            _context.SaveChanges();

            return true;
        }

        public bool UnblockUser(int userId, User user)
        {
            User userToUnBlock = this.GetById(userId);

            if (userToUnBlock == null)
            {
                throw new EntityNotFoundException($"User was not found.");
            }

            if (user.IsAdmin != true)
            {
                throw new NotAuthorizedException("Not authorized. Only admins can block users.");
            }



            userToUnBlock.IsBlocked = false;
            _context.SaveChanges();

            return true;
        }

        private IQueryable<User> GetUsers()
        {
            return this._context.Users;
        }

        public User GetById(int id)
        {
            User user = this.GetUsers().Include(x => x.Cards).Include(u => u.UserWallets).Include(x => x.SavingWallets).Include(x => x.Friends).AsNoTracking().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public User GetUserById(int id)
        {
            User user = this.GetUsers().Include(x => x.Cards).Include(u => u.UserWallets).Include(x => x.SavingWallets).Include(x => x.Friends).FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public bool UploadPhotoVerification(string selfie, string idPhoto, User user)
        {
            VerificationApply newApply = new VerificationApply();

            newApply.Selfie = selfie;
            newApply.IdPhoto = idPhoto;
            newApply.User = user;

            //if (_context.VerificationsApplies.Contains(newApply))
            //{
            //    return false;
            //}

            foreach (var apply in _context.VerificationsApplies)
            {
                if (apply.User.Username == user.Username)
                {
                    return false;
                }
            }

            _context.VerificationsApplies.Add(newApply);

            _context.SaveChanges();

            return true;
        }

        public List<VerificationApply> GetAllVereficationApplies()
        {
            return _context.VerificationsApplies.Include(x => x.User).ToList();
        }

        public void UpdateUserVerification(User user, string text)
        {
            if (text == "Accept")
            {
                user.AdminVerified = true;
            }
            else
            {
                user.AdminVerified = false;
            }

            RemoveApply(user);
            _context.SaveChanges();
        }

        public void RemoveApply(User user)
        {

            foreach (var apply in _context.VerificationsApplies)
            {
                if (apply.User.Username == user.Username)
                {
                    _context.VerificationsApplies.Remove(apply);
                    break;
                }
            }
        }

        public void AddUserCard(Card card, User user)
        {
            // Ensure the user entity is attached to the context
            if (!_context.Users.Contains(user))
            {
                _context.Users.Attach(user);
            }

            // Load the user's cards if not already loaded
            _context.Entry(user).Collection(u => u.Cards).Load();

            // Add the card to the user's card list
            if (user.Cards == null)
            {
                user.Cards = new List<Card>();
            }
            user.Cards.Add(card);

            // Set the foreign key for the card
            card.UserId = user.Id;

            // Save changes and handle potential exceptions
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
            }
        }

        public bool UserEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool UserNameExists(string name)
        {
            return _context.Users.Any(u => u.Username == name);
        }

        public bool UserPhoneNumberExists(string phoneNumber)
        {
            return _context.Users.Any(u => u.PhoneNumber == phoneNumber);
        }

        private static IQueryable<User> FilterByUserName(IQueryable<User> users, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return users.Where(u => u.Username.Contains(username));
            }
            else
            {
                return users;
            }
        }

        private static IQueryable<User> FilterByEmail(IQueryable<User> users, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return users.Where(u => u.Email.Contains(email));
            }
            else
            {
                return users;
            }
        }

        private static IQueryable<User> FilterByNumber(IQueryable<User> users, string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                return users.Where(u => u.PhoneNumber.Contains(phoneNumber));
            }
            else
            {
                return users;
            }
        }

        private static IQueryable<User> SortBy(IQueryable<User> users, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "username":
                    return users.OrderBy(u => u.Username);
                default:
                    return users;
            }
        }

        private static IQueryable<User> OrderBy(IQueryable<User> users, string orderCriteria)
        {
            if (orderCriteria == "desc")
            {
                return users.Reverse();
            }
            else
            {
                return users;
            }
        }

        public List<User> FilterBy(UserQueryParameters filterParameters)
        {
            IQueryable<User> res = this.GetUsers();

            res = FilterByEmail(res, filterParameters.Email);
            res = FilterByUserName(res, filterParameters.Username);
            res = FilterByNumber(res, filterParameters.PhoneNumber);
            res = SortBy(res, filterParameters.SortBy);
            res = OrderBy(res, filterParameters.OrderBy);

            return res.ToList();
        }

        public User FindRecipient(UserQueryParameters recipientDTO)
        {
            User user = new User();

            if (IsValidEmail(recipientDTO.Email))
            {
                user = GetByEmail(recipientDTO.Email);
            }
            else if (IsPhoneNumber(recipientDTO.PhoneNumber))
            {
                user = GetByPhoneNumber(recipientDTO.PhoneNumber);
            }
            else
            {
                user = GetByUsername(recipientDTO.Username);
            }

            return user;

            //user = GetByPhoneNumber(recipientDTO.PhoneNumber);
            //if (user != null)
            //{
            //    return user;
            //}
            //user = GetByUsername(recipientDTO.Username);
            //if (user != null)
            //{
            //    return user;
            //}
            //user = GetByEmail(recipientDTO.Email);


            //return user;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsPhoneNumber(string input)
        {
            return input.All(char.IsDigit); // assuming phone numbers are numeric
        }

        public void AddFriend(int userId, int friendId)
        {
            var user = GetUserById(userId);
            var friend = GetUserById(friendId);

            if (user == null || friend == null)
            {
                throw new EntityNotFoundException("User or friend not found.");
            }

            user.Friends.Add(friend);
            _context.SaveChanges();
        }

        public void RemoveFriend(int userId, int friendId)
        {
            var user = GetUserById(userId);
            var friend = GetUserById(friendId);

            if (user == null || friend == null)
            {
                throw new EntityNotFoundException("User or friend not found.");
            }

            user.Friends.Remove(friend);
            _context.SaveChanges();
        }

        public List<User> GetFriends(int userId)
        {
            var user = GetUserById(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with id={userId} doesn't exist.");
            }

            return user.Friends.ToList();
        }
    }
}
