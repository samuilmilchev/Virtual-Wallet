using Microsoft.EntityFrameworkCore;
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

        public void AddFunds(decimal amount, Wallet wallet)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
            }

            var currentWallet = _context.Wallets.Include(w => w.TransactionHistory).FirstOrDefault(w => w.Id == wallet.Id);
            if (currentWallet == null)
            {
                throw new ArgumentException("Wallet not found", nameof(wallet));
            }

            wallet.Balance += amount;

            var transaction = new Transaction(DateTime.Now , amount , TransactionType.Add , currentWallet.Id);
            currentWallet.TransactionHistory.Add(transaction);
            _context.SaveChanges();
        }

        public decimal ConvertFunds()
        {
            throw new NotImplementedException();
        }

        public Wallet Create(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            _context.SaveChanges();

            return wallet;
        }

        public decimal GetBalance(Wallet wallet)
        {
            return wallet.Balance;
        }

        public void WithdrawFunds(decimal amount, Wallet wallet)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));
            }
            if (amount > wallet.Balance)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the withdrawal!");
            }

            var currentWallet = _context.Wallets.Include(w => w.TransactionHistory).FirstOrDefault(w => w.Id == wallet.Id);
            if (currentWallet == null)
            {
                throw new ArgumentException("Wallet not found", nameof(wallet));
            }

            wallet.Balance -= amount;

            var transaction = new Transaction(DateTime.Now , amount , TransactionType.Withdraw , currentWallet.Id);
            currentWallet.TransactionHistory.Add(transaction);
            _context.SaveChanges();
        }

        //public bool UserHasWallet(string username)   може и да е нужен подобен метод, но на този етап не ни трябва
        //{
            
        //}
    }
}
