using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class GetByUserId_Should
	{
		[TestMethod]
		public void ReturnCorrectCard_When_UserIdIsValid()
		{
			// Arrange
			Card expectedCard = TestHelper.GetTestCard();

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetByUserId(expectedCard.UserId))
				.Returns(expectedCard);

			var sut = new CardService(repositoryMock.Object);

			// Act
			Card actualCard = sut.GetByUserId(expectedCard.UserId);

			// Assert
			Assert.AreEqual(expectedCard, actualCard);
		}
	}
}