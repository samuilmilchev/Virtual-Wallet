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
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>()
            //.HasOne(u => u.UserWallets) // Navigation property on User
            //.WithOne(w => w.Owner) // Navigation property on Wallet
            //.HasForeignKey<Wallet>(w => w.OwnerId); // Specify the foreign key on Wallet

            modelBuilder.Entity<User>()
           .HasMany(u => u.Cards)
           .WithOne(c => c.User)
           .HasForeignKey(c => c.UserId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany(u => u.FriendOf)
                .UsingEntity<Dictionary<string, object>>(
                    "Friends", // Name of the join table
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                );


            var users = new List<User>
            {
            new User { Id = 1, Email = "samuil@example.com", Username = "Samuil", /*Password = ""*/ PhoneNumber = "0845965847", IsAdmin = true, IsBlocked = false, Role = UserRole.User},
            new User { Id = 2, Email = "violin@example.com", Username = "Violin", /*Password = ""*/PhoneNumber = "0865214587", IsAdmin = true, IsBlocked = false, Role = UserRole.User},
            new User { Id = 3, Email = "alex@example.com", Username = "Alex", /*Password = ""*/ PhoneNumber = "0826541254", IsAdmin = true, IsBlocked = false, Role = UserRole.User},
            };

            modelBuilder.Entity<User>().HasData(users);

            var cards = new List<Card>
            {
                new Card {Id = 1, CardHolder = "Samuil Milchev", CardNumber = "359039739152721", CheckNumber = "111", ExpirationData = "10/28", UserId = 1},
                new Card {Id = 2, CardHolder = "Violin Filev", CardNumber = "379221059046032", CheckNumber = "112", ExpirationData = "04/28", UserId = 1},
                new Card {Id = 3, CardHolder = "Alexander Georgiev", CardNumber = "345849306009469", CheckNumber = "121", ExpirationData = "02/28", UserId = 1}
            };

            modelBuilder.Entity<Card>().HasData(cards);

            var wallets = new List<Wallet>
            {
                new Wallet
                {
                    Id = 1,
                    OwnerId = 1,
                    WalletName = "Violin's wallet",
                    Amount = 1000m,
                    Currency = Currency.BGN
                },
                new Wallet
                {
                    Id = 2,
                    OwnerId = 2,
                    WalletName = "Sami's wallet",
                    Amount = 1000m,
                    Currency = Currency.BGN
                },
                new Wallet
                {
                    Id = 3,
                    OwnerId = 3,
                    WalletName = "Alex's wallet",
                    Amount = 1000m,
                    Currency = Currency.BGN
                }
            };

            modelBuilder.Entity<Wallet>().HasData(wallets);

            // modelBuilder.Entity<Wallet>()
            //.Property(w => w.RowVersion)
            //.IsRowVersion();  // Configure RowVersion as a concurrency token

            // Transaction entity configuration
            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.Id); // Ensure Id is the primary key

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Timestamp)
                .IsRequired(); // Ensure Timestamp is required

            // Any other Transaction-specific configuration goes here
        }
    }
}
