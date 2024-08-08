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

        public List<User> GetAll()
        {
            return this.GetUsers().ToList();
        }

        public User GetByEmail(string email)
        {
            User user = this.GetUsers().FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with e-mail: {email} does not exist!");
            }
            return user;
        }
        public User GetByUsername(string username)
        {
            User user = this.GetUsers().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with username {username} does not exist!");
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

            /*Include(u => u.Id).*/
            //Include(u => u.Username)      //moje i da ne sa nujni tezi include-ove
            //.Include(u => u.FirstName)
            //.Include(u => u.LastName)
            //.Include(u=>u.Email)
            //.Include(u => u.IsAdmin)
            //.Include(u => u.IsBlocked); 
        }

        public User GetById(int id)
        {
            User user = this.GetUsers().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User with id={id} doesn't exist.");
        }

        public bool UserEmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
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

        private static IQueryable<User> FilterByUsername(IQueryable<User> users, string username)
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

            res = FilterByUsername(res, filterParameters.Firstname);
            res = FilterByEmail(res, filterParameters.Email);
            res = FilterByUserName(res, filterParameters.Username);
            res = SortBy(res, filterParameters.SortBy);
            res = OrderBy(res, filterParameters.OrderBy);

            return res.ToList();
        }
    }
}
