using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface ICardService
    {
        public Card Create(Card card);
        public bool Delete(int id, User user);
        public List<Card> GetAll();
        public Card GetByCardHoler(string cardHolder);
        Card GetByUserId(int userId); //needed for transfers between card and wallet
        public Card GetById(int id);
        decimal GetBalance(string cardNumber);
        Card UpdateCardBalance(int id, Card card);
    }
}
