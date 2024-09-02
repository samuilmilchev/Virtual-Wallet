using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class FriendsListViewModel
    {
        public string input {  get; set; }
        public ICollection<string> friends { get; set; }
    }
}
