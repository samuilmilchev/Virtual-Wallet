﻿using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Services.Contracts
{
    public interface ICardService
    {
        public Card Create(Card card);
        public bool Delete(int id, User user);
        public List<Card> GetAll();
        public Card GetByCardHoler(string cardHolder);
        public Card GetById(int id);
        double GetBalance(string cardNumber);
    }
}
