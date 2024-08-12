using System.Text.RegularExpressions;
using Virtual_Wallet.Db;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationContext _context;

        public WalletRepository(ApplicationContext context)
        {
            this._context = context;
        }

        public void AddFunds(double amount, Wallet wallet)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
            }
            wallet.Balance += amount;
            _context.SaveChanges();
        }

        public double ConvertFunds()
        {
            throw new NotImplementedException();
        }

        public Wallet Create(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return wallet;
        }

        public double GetBalance(Wallet wallet)
        {
            return wallet.Balance;
        }

        public void WithdrawFunds(double amount, Wallet wallet)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
            }
            if (amount > wallet.Balance)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the withdrawal!");
            }

            wallet.Balance -= amount;
            _context.SaveChanges();
        }

        //public bool UserHasWallet(string username)   може и да е нужен подобен метод, но на този етап не ни трябва
        //{
            
        //}
    }
}
