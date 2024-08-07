using Virtual_Wallet.Db;
using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly ApplicationContext _context;

        public CardRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Card Create(Card card)
        {
            _context.Cards.Add(card);
            _context.SaveChanges();

            return card;
        }

        public bool Delete(int id)
        {
            Card card = this.GetById(id);
            _context.Cards.Remove(card);
            _context.SaveChanges();

            return true;
        }

        public List<Card> GetAll()
        {
            return this.GetCards().ToList();
        }

        public Card GetByCardHoler(string cardHolder)
        {
            Card card = this.GetCards().FirstOrDefault(x => x.CardHolder == cardHolder);
            if (card == null)
            {
                throw new EntityNotFoundException($"User with first name: {card.CardHolder} does not exist!");
            }
            return card;
        }

        private IQueryable<Card> GetCards()
        {
            return this._context.Cards;

            /*Include(u => u.Id).*/
            //Include(u => u.Username)
            //.Include(u => u.FirstName)
            //.Include(u => u.LastName)
            //.Include(u=>u.Email)
            //.Include(u => u.IsAdmin)
            //.Include(u => u.IsBlocked); 
        }

        public Card GetById(int id)
        {
            Card card = this.GetCards().FirstOrDefault(u => u.Id == id);

            return card ?? throw new EntityNotFoundException($"Card with id={id} doesn't exist.");
        }

        public Card GetByCardNumber(string cardNumber)
        {
            Card card = this.GetCards().FirstOrDefault(x => x.CardNumber == cardNumber);

            return card;
        }

        //public bool BlockCard(int userId, Card card)
        //{
        //}

        //public bool UnblockCard(int userId, Card card)
        //{
        //}

        //private static IQueryable<Card> FilterByType()
        //{
        //}

        //private static IQueryable<Card> SortBy()
        //{
        //}

        //private static IQueryable<Card> OrderBy()
        //{
        //}

        //public List<Card> FilterBy(UserQueryParameters filterParameters)
        //{
        //    IQueryable<User> res = this.GetUsers();

        //    res = FilterByFirstName(res, filterParameters.Firstname);
        //    res = FilterByEmail(res, filterParameters.Email);
        //    res = FilterByUserName(res, filterParameters.Username);
        //    res = SortBy(res, filterParameters.SortBy);
        //    res = OrderBy(res, filterParameters.OrderBy);

        //    return res.ToList();
        //}
    }
}
