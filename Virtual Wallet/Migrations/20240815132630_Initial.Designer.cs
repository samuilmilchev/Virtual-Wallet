﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Virtual_Wallet.Db;

#nullable disable

namespace Virtual_Wallet.Migrations
{
    [DbContext(typeof(ApplicationContext))]
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
    [Migration("20240818183655_Initial")]
========
    [Migration("20240815132630_Initial")]
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
========
                    b.Property<string>("CardHolder")
                        .HasColumnType("nvarchar(max)");

>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.Property<string>("CheckNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpirationData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 100000m,
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
========
                            CardHolder = "Samuil Milchev",
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                            CardNumber = "359039739152721",
                            CardType = 0,
                            CheckNumber = "111",
                            ExpirationData = "10/28",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Balance = 100000m,
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
========
                            CardHolder = "Violin Filev",
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                            CardNumber = "379221059046032",
                            CardType = 0,
                            CheckNumber = "112",
                            ExpirationData = "04/28",
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
                            UserId = 2
========
                            UserId = 1
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                        },
                        new
                        {
                            Id = 3,
                            Balance = 100000m,
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
========
                            CardHolder = "Alexander Georgiev",
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                            CardNumber = "345849306009469",
                            CardType = 0,
                            CheckNumber = "121",
                            ExpirationData = "02/28",
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
                            UserId = 3
========
                            UserId = 1
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

========
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "samuil@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            PhoneNumber = "0845965847",
                            Role = 1,
                            Username = "Samuil",
                            WalletId = 0
                        },
                        new
                        {
                            Id = 2,
                            Email = "violin@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            PhoneNumber = "0865214587",
                            Role = 1,
                            Username = "Violin",
                            WalletId = 0
                        },
                        new
                        {
                            Id = 3,
                            Email = "alex@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            PhoneNumber = "0826541254",
                            Role = 1,
                            Username = "Alex",
                            WalletId = 0
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("WalletName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Wallets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 0.00m,
                            OwnerId = 1,
                            WalletName = "Violin's wallet"
                        },
                        new
                        {
                            Id = 2,
                            Balance = 0.00m,
                            OwnerId = 2,
                            WalletName = "Sami's wallet"
                        },
                        new
                        {
                            Id = 3,
                            Balance = 0.00m,
                            OwnerId = 3,
                            WalletName = "Alex's wallet"
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Card", b =>
                {
<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "CardHolder")
========
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "User")
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

<<<<<<<< HEAD:Virtual Wallet/Migrations/20240818183655_Initial.Designer.cs
                    b.Navigation("CardHolder");
========
                    b.Navigation("User");
>>>>>>>> master:Virtual Wallet/Migrations/20240815132630_Initial.Designer.cs
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Transaction", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.Wallet", "Wallet")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Wallet", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "Owner")
                        .WithOne("UserWallet")
                        .HasForeignKey("Virtual_Wallet.Models.Entities.Wallet", "OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.User", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("UserWallet");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Wallet", b =>
                {
                    b.Navigation("TransactionHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
