using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card Create(Card card)
        {
            if (_cardRepository.GetByCardNumber(card.CardNumber) != null)
            {
                throw new DuplicateEntityException($"A card with the same number already exists !");
            }

            Card createdCard = _cardRepository.Create(card);
            return createdCard;
        }

        public bool Delete(int id, User user)
        {
            Card cardToDelete = _cardRepository.GetById(id);

            if (user.Username != cardToDelete.CardHolder)
            {
                throw new NotAuthorizedException("You are not authorised to delete. Only admin or the owner of this user can delete!");
            }

            return _cardRepository.Delete(id);
        }

        public List<Card> GetAll()
        {
            return _cardRepository.GetAll();
        }

        public Card GetByCardHoler(string cardHolder)
        {
            return _cardRepository.GetByCardHoler(cardHolder);
        }

        public Card GetById(int id)
        {
            return _cardRepository.GetById(id);
        }
    }
}
