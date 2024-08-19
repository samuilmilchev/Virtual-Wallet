using Virtual_Wallet.Exceptions;
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

        public void AddFunds(decimal amount, Wallet wallet , Card card) //from card to wallet
        {
            if (card.Balance < amount)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
            }
            card.Balance -= amount;
            this.walletRepository.AddFunds(amount, wallet);
        }

        public decimal ConvertFunds()
        {
            throw new NotImplementedException();
        }

        public Wallet Create(Wallet wallet)
        {
            return walletRepository.Create(wallet); 
        }

        public decimal GetBalance(Wallet wallet)
        {
            return this.walletRepository.GetBalance(wallet);
        }

        public void WithdrawFunds(decimal amount, Wallet wallet, Card card) //from wallet to card
        {
            this.walletRepository.WithdrawFunds(amount , wallet);
            card.Balance += amount;
            this.cardService.UpdateCardBalance(card.Id,card); //нужно за да се запази стойността в базата 
        }
    }
}
