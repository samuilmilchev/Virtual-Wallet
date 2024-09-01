using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Helpers;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Models.ViewModels;
using Virtual_Wallet.Repository;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services.Contracts;

namespace Virtual_Wallet.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;
        private readonly ICardService cardService;
        private readonly Currencyapi currencyapi;
        private readonly IUsersService usersService;
        private readonly IExchangeRateService exchangeRateService; // Assuming you have this service for currency conversion
        private readonly IEmailService emailService;

        public WalletService(IWalletRepository walletRepository, ICardService cardService, Currencyapi currencyapi, IUsersService usersService, IEmailService emailService,  IExchangeRateService exchangeRateService)
        {
            this.walletRepository = walletRepository;
            this.cardService = cardService;
            this.currencyapi = currencyapi;
            this.usersService = usersService;
            this.emailService = emailService;
            this.exchangeRateService = exchangeRateService;
        }

        public void AddFunds(decimal amount, Currency currency, Wallet wallet, Card card, User user) // From card to wallet
        {
            if (card.Balance < amount)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
            }
            card.Balance -= amount;
            this.walletRepository.AddFunds(amount, currency, wallet, user);
        }

        public void ConvertFunds(decimal amount, Currency fromCurrency, Currency toCurrency, string username/*Wallet fromWallet, Wallet toWallet, User user*/)
        {
            User currentUser = usersService.GetByUsername(username);

            Wallet fromWallet = GetByCurrency(fromCurrency, currentUser);

            Wallet toWallet = GetByCurrency(toCurrency, currentUser);

            if (fromCurrency == toCurrency)
            {
                throw new InvalidOperationException("Cannot convert between the same currency.");
            }

            //if (fromWallet.Amount < amount)
            //{
            //	throw new InsufficientFundsException("Insufficient funds in the source currency to execute the conversion.");
            //}

            string JsonExchangeRateReturn = currencyapi.Latest(fromCurrency, toCurrency);

            string[] splitResponse = JsonExchangeRateReturn.Split(':');

            string stringedConversionRate = splitResponse[2].TrimEnd('}');

            decimal exchangeRate = decimal.Parse(stringedConversionRate);

            var convertedAmount = amount * exchangeRate;

            walletRepository.WithdrawFunds(amount, fromWallet);
            walletRepository.AddFunds(convertedAmount, toCurrency, toWallet, currentUser);
        }

        public Wallet Create(Wallet wallet)
        {
            return walletRepository.Create(wallet);
        }

        //public decimal GetBalance(Wallet wallet, Currency currency)
        //{
        //	return this.walletRepository.GetBalance(wallet, currency);
        //}

        public void WithdrawFunds(decimal amount, Wallet wallet, Card card) // From wallet to card
        {
            if (wallet.Amount < amount)
            {
                throw new InsufficientFundsException("Insufficient funds to execute the transaction!");
            }

            this.walletRepository.WithdrawFunds(amount, wallet);
            card.Balance += amount;
            this.cardService.UpdateCardBalance(card.Id, card);
        }

        //New method: Transfer funds from one wallet to another
        public void TransferFunds(decimal amount, Currency currency, Wallet fromWallet, Wallet toWallet, User user)
        {
            walletRepository.SendMoney(amount, currency, fromWallet, toWallet, user);
        }

        public Wallet GetByCurrency(Currency currency, User user)
        {
            foreach (var wallet in user.UserWallets)
            {
                if (wallet.Currency.Equals(currency))
                {
                    return wallet;
                }
            }
            var newWallet = new Wallet();
            newWallet.Currency = currency;
            newWallet.Owner = user;
            newWallet.WalletName = $"{currency} Wallet";
            user.UserWallets.Add(newWallet);
            return newWallet;
        }


        public void CreateSavingWallet(SavingWalletViewModel model)
        {
            walletRepository.CreateSavingWallet(model);
        }

        public decimal CalculateInterest(SavingWalletViewModel model)
        {
            double totalDays = (model.EndDate - DateTime.Now).TotalDays;

            decimal interest = model.Amount * 0.01m;

            decimal result = Math.Round((decimal)totalDays * interest, 4, MidpointRounding.ToEven);
            return result;
        }

        public decimal CalculateTotal(SavingWalletViewModel model)
        {
            double totalDays = (model.EndDate - DateTime.Now).TotalDays;
            decimal calculatedInterest = CalculateInterest(model) * (decimal)totalDays + model.Amount;

            var result = Math.Round(calculatedInterest, 1, MidpointRounding.ToEven);
            return result;
        }

        public string GenerateEmailConfirationToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public async Task SendConfirmationEmailAsync(User user)
        {
            var token = GenerateEmailConfirationToken();
            user.TransactionVerificationToken = token;
            user.TransactionTokenExpiry = DateTime.Now.AddMinutes(10);

            usersService.Update(user.Id , user);
            await emailService.SendAsync(user.Email, "Transaction Verification", $"This is your transaction verification code: {token} .<p>Please do not share this code to anyone.</p>");


        }
    }
}