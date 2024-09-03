using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virtual_Wallet.Models.Entities;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	public class TestHelper
	{
		public static Card GetTestCard()
		{
			return new Card()
			{
				Id = 1,
				CardNumber = "359039739152721",
				ExpirationData = "10/28",
				CardHolder = "Justine Fox",
				CheckNumber = "111",
				CardType = CardType.Credit,
				Balance = 1000m,
				UserId = 1,
			};
		}
	}
}

//public int Id { get; set; }
//public string CardNumber { get; set; }
//public string ExpirationData { get; set; }
//public string CardHolder { get; set; }
//public string CheckNumber { get; set; }
//public CardType CardType { get; set; }
//public decimal Balance { get; set; } = 100000m;

//// Add this line to create a relationship to the User
//public int UserId { get; set; } // Foreign key to User
//public User User { get; set; }  // Navigation property back to User

//new Card { Id = 1, CardHolder = "Justine Fox", CardNumber = "359039739152721", CheckNumber = "111", ExpirationData = "10/28", UserId = 1 },
