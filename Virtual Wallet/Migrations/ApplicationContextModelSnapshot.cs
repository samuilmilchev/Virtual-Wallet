﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Virtual_Wallet.Db;

#nullable disable

namespace Virtual_Wallet.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("CardHolder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CardHolderId")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.Property<int>("CheckNumber")
                        .HasColumnType("int");

                    b.Property<string>("ExpirationData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CardHolder = "Samuil Milchev",
                            CardHolderId = 0,
                            CardNumber = "359039739152721",
                            CardType = 0,
                            CheckNumber = 111,
                            ExpirationData = "10/28"
                        },
                        new
                        {
                            Id = 2,
                            CardHolder = "Violin Filev",
                            CardHolderId = 0,
                            CardNumber = "379221059046032",
                            CardType = 0,
                            CheckNumber = 112,
                            ExpirationData = "04/28"
                        },
                        new
                        {
                            Id = 3,
                            CardHolder = "Alexander Georgiev",
                            CardHolderId = 0,
                            CardNumber = "345849306009469",
                            CardType = 0,
                            CheckNumber = 121,
                            ExpirationData = "02/28"
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
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
                            PhoneNumber = "0845965847",
                            Username = "Samuil"
                        },
                        new
                        {
                            Id = 2,
                            Email = "violin@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            PhoneNumber = "0865214587",
                            Username = "Violin"
                        },
                        new
                        {
                            Id = 3,
                            Email = "alex@example.com",
                            IsAdmin = true,
                            IsBlocked = false,
                            PhoneNumber = "0826541254",
                            Username = "Alex"
                        });
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.Card", b =>
                {
                    b.HasOne("Virtual_Wallet.Models.Entities.User", null)
                        .WithMany("Card")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Virtual_Wallet.Models.Entities.User", b =>
                {
                    b.Navigation("Card");
                });
#pragma warning restore 612, 618
        }
    }
}
