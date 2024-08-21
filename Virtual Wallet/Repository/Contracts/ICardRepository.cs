﻿using Virtual_Wallet.DTOs.UserDTOs;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Repository.Contracts
{
    public interface ICardRepository
    {
        List<Card> GetAll();
        Card GetByCardHoler(string cardHolder);
        Card Create(Card card);
        bool Delete(int id);
        Card GetById(int id);
        Card GetByCardNumber(string cardNumber);
        decimal GetBalance(string cardNumber);
        Card UpdateCardBalance(int id, Card card);
        Card GetByUserId(int userId);
        //bool BlockCard(int userId, User user);
        //bool UnblockCard(int userId, User user);
        //List<User> FilterBy(UserQueryParameters filterParameters);
    }
}
