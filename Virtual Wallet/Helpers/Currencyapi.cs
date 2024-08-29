using System.Data.SqlTypes;
using System;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Helpers
{
    public class Currencyapi
    {
        private string ApiKey { get; } = "fca_live_FnDPZgLdeSRfq0vdWySRx83mkxo0ZW3y9g70y0aZ";

        public Currencyapi()
        {
        }

        public string Status()
        {
            return RequestHelper.Status(ApiKey);
        }

        public string Currencies(Currency currencies)
        {
            return RequestHelper.Currencies(ApiKey, currencies);
        }

        public string Latest(Currency baseCurrency, Currency currencies)
        {
            return RequestHelper.Latest(ApiKey, baseCurrency, currencies);
        }

        public string Historical(string data, Currency baseCurrency, Currency currencies)
        {
            return RequestHelper.Historical(ApiKey, data, baseCurrency, currencies);
        }
    }
}
