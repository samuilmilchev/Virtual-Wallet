using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Models.ViewModels
{
    public class UserPViewModel
    {
        public int currentPageIndex { get; set; }
        public int pageCount { get; set; }
        public List<User> UserList { get; set; }
    }
}
