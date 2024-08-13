﻿using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;
        private readonly ICardService cardService;

        public WalletService(IWalletRepository walletRepository, ICardService cardService)
        {
            this.walletRepository = walletRepository;
            this.cardService = cardService;
        }

        public void AddFunds(double amount, Wallet wallet , Card card) //from card to wallet
        {
            if (card.Balance < amount)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
            }
            card.Balance -= amount;
            this.walletRepository.AddFunds(amount, wallet);
        }

        public double ConvertFunds()
        {
            throw new NotImplementedException();
        }

        public Wallet Create(Wallet wallet)
        {
            return walletRepository.Create(wallet); 
        }

        public double GetBalance(Wallet wallet)
        {
            return this.walletRepository.GetBalance(wallet);
        }

        public void WithdrawFunds(double amount, Wallet wallet, Card card) //from wallet to card
        {
            this.walletRepository.WithdrawFunds(amount , wallet);
            card.Balance += amount;
        }
    }
}
