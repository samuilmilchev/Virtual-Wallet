using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class GetBalance_Should
	{
		[TestMethod]
		public void ReturnCorrectBalance_When_CardNumberIsValid()
		{
			// Arrange
			Card testCard = TestHelper.GetTestCard();
			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetBalance(testCard.CardNumber))
				.Returns(testCard.Balance);

			var sut = new CardService(repositoryMock.Object);

			// Act
			var balance = sut.GetBalance(testCard.CardNumber);

			// Assert
			Assert.AreEqual(testCard.Balance, balance);
		}
	}
}