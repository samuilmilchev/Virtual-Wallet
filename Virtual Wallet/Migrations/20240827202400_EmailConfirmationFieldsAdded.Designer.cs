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
    [Migration("20240827202400_EmailConfirmationFieldsAdded")]
    partial class EmailConfirmationFieldsAdded
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

                    b.Property<string>("CardHolder")
                        .HasColumnType("nvarchar(max)");

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
                            CardHolder = "Samuil Milchev",
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
                            CardHolder = "Violin Filev",
                            CardNumber = "379221059046032",
                            CardType = 0,
                            CheckNumber = "112",
                            ExpirationData = "04/28",
                            UserId = 1
                        },
                        new
                        {
                            Id = 3,
                            Balance = 100000m,
                            CardHolder = "Alexander Georgiev",
                            CardNumber = "345849306009469",
                            CardType = 0,
                            CheckNumber = "121",
                            ExpirationData = "02/28",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<int?>("RecipientId")
                        .HasColumnType("int");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipientId");

                    b.HasIndex("SenderId");

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

                    b.Property<string>("EmailConfirmationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EmailTokenExpiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailVerified")
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

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "samuil@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            IsEmailVerified = false,
                            PhoneNumber = "0845965847",
                            Role = 1,
                            Username = "Samuil"
                        },
                        new
                        {
                            Id = 2,
                            Email = "violin@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            IsEmailVerified = false,
                            PhoneNumber = "0865214587",
                            Role = 1,
                            Username = "Violin"
                        },
                        new
                        {
                            Id = 3,
                            Email = "alex@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            IsEmailVerified = false,
                            PhoneNumber = "0826541254",
                            Role = 1,
                            Username = "Alex"
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("WalletName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Wallets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 1000m,
                            Currency = 0,
                            OwnerId = 1,
                            WalletName = "Violin's wallet"
                        },
                        new
                        {
                            Id = 2,
                            Amount = 1000m,
                            Currency = 0,
                            OwnerId = 2,
                            WalletName = "Sami's wallet"
                        },
                        new
                        {
                            Id = 3,
                            Amount = 1000m,
                            Currency = 0,
                            OwnerId = 3,
                            WalletName = "Alex's wallet"
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Card", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "User")
                        .WithMany("Cards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Transaction", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.HasOne("Virtual_Wallet.Models.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.HasOne("Virtual_Wallet.Models.Entities.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipient");

                    b.Navigation("Sender");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Wallet", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.User", "Owner")
                        .WithMany("UserWallets")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.User", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("UserWallets");
                });
#pragma warning restore 612, 618
        }
    }
}
