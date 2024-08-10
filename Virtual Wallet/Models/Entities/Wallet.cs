namespace Virtual_Wallet.Models.Entities
{
	public class Wallet : IWallet
	{
		// Properties
		public string WalletId { get; private set; }
		public Dictionary<string, string> OwnerInfo { get; private set; }
		public Dictionary<string, decimal> Balances { get; private set; }
		public Dictionary<string, Dictionary<string, decimal>> ExchangeRates { get; private set; }
		public string CurrentCurrency { get; private set; }
		public List<Transaction> TransactionHistory { get; private set; }

		// Constructor
		public Wallet(string walletId, string ownerId, string ownerName, Dictionary<string, decimal> initialBalances, string currentCurrency)
		{
			if (string.IsNullOrEmpty(walletId))
				throw new ArgumentException("Wallet ID cannot be null or empty", nameof(walletId));
			if (string.IsNullOrEmpty(ownerId))
				throw new ArgumentException("Owner ID cannot be null or empty", nameof(ownerId));
			if (string.IsNullOrEmpty(ownerName))
				throw new ArgumentException("Owner name cannot be null or empty", nameof(ownerName));
			if (string.IsNullOrEmpty(currentCurrency))
				throw new ArgumentException("Current currency cannot be null or empty", nameof(currentCurrency));
			if (initialBalances == null || !initialBalances.ContainsKey(currentCurrency))
				throw new ArgumentException("Initial balances must include the current currency");

			WalletId = walletId;

			OwnerInfo = new Dictionary<string, string>
		{
			{ ownerId, ownerName }
		};

			Balances = initialBalances;
			CurrentCurrency = currentCurrency;
			TransactionHistory = new List<Transaction>();

			ExchangeRates = new Dictionary<string, Dictionary<string, decimal>>();
		}

		// Method to add funds in the current currency
		public void AddFunds(decimal amount)
		{
			AddFunds(CurrentCurrency, amount);
		}

		// Method to add funds to a specific currency balance
		public void AddFunds(string currency, decimal amount)
		{
			if (string.IsNullOrEmpty(currency))
				throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
			if (amount <= 0)
				throw new ArgumentException("Amount to add must be greater than zero", nameof(amount));

			if (Balances.ContainsKey(currency))
			{
				Balances[currency] += amount;
			}
			else
			{
				Balances[currency] = amount;
			}

			// Record the transaction
			TransactionHistory.Add(new Transaction(DateTime.Now, currency, amount, TransactionType.Add));
		}

		// Method to withdraw funds in the current currency
		public void WithdrawFunds(decimal amount)
		{
			WithdrawFunds(CurrentCurrency, amount);
		}

		// Method to withdraw funds from a specific currency balance
		public void WithdrawFunds(string currency, decimal amount)
		{
			if (string.IsNullOrEmpty(currency))
				throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
			if (amount <= 0)
				throw new ArgumentException("Amount to withdraw must be greater than zero", nameof(amount));
			if (!Balances.ContainsKey(currency) || Balances[currency] < amount)
				throw new InvalidOperationException("Insufficient funds in the specified currency");

			Balances[currency] -= amount;

			// Record the transaction
			TransactionHistory.Add(new Transaction(DateTime.Now, currency, amount, TransactionType.Withdraw));
		}

		// Method to get the balance for the current currency
		public decimal GetBalance()
		{
			return GetBalance(CurrentCurrency);
		}

		// Method to get the balance for a specific currency
		public decimal GetBalance(string currency)
		{
			if (string.IsNullOrEmpty(currency))
				throw new ArgumentException("Currency cannot be null or empty", nameof(currency));

			return Balances.ContainsKey(currency) ? Balances[currency] : 0;
		}

		// Method to add an exchange rate (base currency -> target currency -> rate)
		public void AddExchangeRate(string baseCurrency, string targetCurrency, decimal rate)
		{
			if (string.IsNullOrEmpty(baseCurrency) || string.IsNullOrEmpty(targetCurrency))
				throw new ArgumentException("Currency codes cannot be null or empty");
			if (rate <= 0)
				throw new ArgumentException("Exchange rate must be greater than zero");

			if (!ExchangeRates.ContainsKey(baseCurrency))
			{
				ExchangeRates[baseCurrency] = new Dictionary<string, decimal>();
			}

			ExchangeRates[baseCurrency][targetCurrency] = rate;
		}

		// Method to convert funds to the current currency
		public void ConvertFunds(string fromCurrency, decimal amount)
		{
			ConvertFunds(fromCurrency, CurrentCurrency, amount);
		}

		// Method to convert funds from one currency to another
		public void ConvertFunds(string fromCurrency, string toCurrency, decimal amount)
		{
			if (string.IsNullOrEmpty(fromCurrency) || string.IsNullOrEmpty(toCurrency))
				throw new ArgumentException("Currency codes cannot be null or empty");
			if (amount <= 0)
				throw new ArgumentException("Amount to convert must be greater than zero");

			if (!Balances.ContainsKey(fromCurrency) || Balances[fromCurrency] < amount)
				throw new InvalidOperationException("Insufficient funds in the source currency");

			if (!ExchangeRates.ContainsKey(fromCurrency) || !ExchangeRates[fromCurrency].ContainsKey(toCurrency))
				throw new InvalidOperationException("Exchange rate not available for the specified currency pair");

			// Perform the conversion
			decimal rate = ExchangeRates[fromCurrency][toCurrency];
			decimal convertedAmount = amount * rate;

			// Update balances
			WithdrawFunds(fromCurrency, amount);
			AddFunds(toCurrency, convertedAmount);

			// Record the transaction
			TransactionHistory.Add(new Transaction(DateTime.Now, toCurrency, convertedAmount, TransactionType.Convert));
		}

		// Method to set the current currency
		public void SetCurrentCurrency(string currency)
		{
			if (string.IsNullOrEmpty(currency))
				throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
			if (!Balances.ContainsKey(currency))
				throw new ArgumentException("The wallet does not contain any balance for the specified currency");

			CurrentCurrency = currency;
		}

		// Method to display wallet information
		public override string ToString()
		{
			string ownerId = OwnerInfo.Keys.Count > 0 ? OwnerInfo.Keys.GetEnumerator().Current : "N/A";
			string ownerName = OwnerInfo.ContainsKey(ownerId) ? OwnerInfo[ownerId] : "N/A";

			string balancesInfo = string.Join(", ", Balances);
			string transactionHistoryInfo = string.Join("\n", TransactionHistory);
			return $"Wallet ID: {WalletId}\nOwner ID: {ownerId}\nOwner Name: {ownerName}\nCurrent Currency: {CurrentCurrency}\nBalances: {balancesInfo}\nTransaction History:\n{transactionHistoryInfo}";
		}
	}
}
