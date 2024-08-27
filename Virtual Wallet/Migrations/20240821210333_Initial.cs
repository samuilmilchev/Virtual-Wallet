﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Virtual_Wallet.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardHolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    WalletName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Image", "IsAdmin", "IsBlocked", "PasswordHash", "PasswordSalt", "PhoneNumber", "Role", "Username" },
                values: new object[] { 1, "samuil@example.com", null, true, false, null, null, "0845965847", 1, "Samuil" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Image", "IsAdmin", "IsBlocked", "PasswordHash", "PasswordSalt", "PhoneNumber", "Role", "Username" },
                values: new object[] { 2, "violin@example.com", null, true, false, null, null, "0865214587", 1, "Violin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Image", "IsAdmin", "IsBlocked", "PasswordHash", "PasswordSalt", "PhoneNumber", "Role", "Username" },
                values: new object[] { 3, "alex@example.com", null, true, false, null, null, "0826541254", 1, "Alex" });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "Balance", "CardHolder", "CardNumber", "CardType", "CheckNumber", "ExpirationData", "UserId" },
                values: new object[,]
                {
                    { 1, 100000m, "Samuil Milchev", "359039739152721", 0, "111", "10/28", 1 },
                    { 2, 100000m, "Violin Filev", "379221059046032", 0, "112", "04/28", 1 },
                    { 3, 100000m, "Alexander Georgiev", "345849306009469", 0, "121", "02/28", 1 }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Amount", "Currency", "OwnerId", "WalletName" },
                values: new object[,]
                {
                    { 1, 1000m, 0, 1, "Violin's wallet" },
                    { 2, 1000m, 0, 2, "Sami's wallet" },
                    { 3, 1000m, 0, 3, "Alex's wallet" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_OwnerId",
                table: "Wallets",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
