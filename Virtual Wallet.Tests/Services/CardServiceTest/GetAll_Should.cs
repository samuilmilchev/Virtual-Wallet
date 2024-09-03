using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using System.Collections.Generic;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class GetAll_Should
	{
		[TestMethod]
		public void ReturnAllCards_When_Called()
		{
			// Arrange
			List<Card> expectedCards = new List<Card> { TestHelper.GetTestCard() };

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetAll())
				.Returns(expectedCards);

			var sut = new CardService(repositoryMock.Object);

			// Act
			var actualCards = sut.GetAll();

			// Assert
			CollectionAssert.AreEqual(expectedCards, actualCards);
		}
	}
}
