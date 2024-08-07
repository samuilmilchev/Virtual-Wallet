using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Service
{
    public class CardService
    {
        private readonly ICardRepository _cardRepository;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public Card Create(Card card)
        {
            if (this._cardRepository.GetByCardNumber(card.CardNumber) != null)
            {
                throw new DuplicateEntityException($"A card with the same number already exists !");
            }

            Card createdCard = this._cardRepository.Create(card);
            return createdCard;
        }

        public bool Delete(int id, User user)
        {
            Card cardToDelete = this._cardRepository.GetById(id);

            var userNames = $"{user.FirstName} + {user.LastName}";

            if (userNames != cardToDelete.CardHolder)
            {
                throw new NotAuthorizedException("You are not authorised to delete. Only admin or the owner of this user can delete!");
            }

            return this._cardRepository.Delete(id);
        }

        public List<Card> GetAll()
        {
            return this._cardRepository.GetAll();
        }

        public Card GetByCardHoler(string cardHolder)
        {
            return this._cardRepository.GetByCardHoler(cardHolder);
        }

        public Card GetById(int id)
        {
            return this._cardRepository.GetById(id);
        }
    }
}
