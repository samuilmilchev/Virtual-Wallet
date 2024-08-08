using Microsoft.EntityFrameworkCore;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Db
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var users = new List<User>
            {
            new User { Id = 1, Email = "samuil@example.com", Username = "Samuil", /*Password = ""*/ PhoneNumber = "0845965847", IsAdmin = true, IsBlocked = false},
            new User { Id = 2, Email = "violin@example.com", Username = "Violin", /*Password = ""*/PhoneNumber = "0865214587", IsAdmin = true, IsBlocked = false},
            new User { Id = 3, Email = "alex@example.com", Username = "Alex", /*Password = ""*/ PhoneNumber = "0826541254", IsAdmin = true, IsBlocked = false},
            };

            modelBuilder.Entity<User>().HasData(users);

            var cards = new List<Card> 
            {
                new Card {Id = 1, CardHolder = "Samuil Milchev", CardNumber = "359039739152721", CheckNumber = 111, ExpirationData = "10/28"},
                new Card {Id = 2, CardHolder = "Violin Filev", CardNumber = "379221059046032", CheckNumber = 112, ExpirationData = "04/28"},
                new Card {Id = 3, CardHolder = "Alexander Georgiev", CardNumber = "345849306009469", CheckNumber = 121, ExpirationData = "02/28"}
            };

            modelBuilder.Entity<Card>().HasData(cards);
        }
    }
}
