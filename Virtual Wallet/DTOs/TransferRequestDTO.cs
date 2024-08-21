using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.DTOs
{
    public class TransferRequestDTO
    {
        //тук трябва да имам променлива която да свързвам крадит/дебит картата с виртуалния портфейл
        //примерно WalletID

        public string TransferType { get; set; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        
    }
}
