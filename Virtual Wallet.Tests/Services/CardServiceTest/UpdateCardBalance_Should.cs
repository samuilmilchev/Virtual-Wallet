using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Virtual_Wallet.Exceptions;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class UpdateCardBalance_Should
	{
		[TestMethod]
		public void UpdateBalanceSuccessfully_When_ParametersAreValid()
		{
			// Arrange
			var initialCard = TestHelper.GetTestCard();
			var updatedCard = TestHelper.GetTestCard();
			updatedCard.Balance = 2000m; // New balance

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(initialCard.Id))
				.Returns(initialCard);
			repositoryMock
				.Setup(c => c.UpdateCardBalance(initialCard.Id, updatedCard))
				.Returns(updatedCard);

			var sut = new CardService(repositoryMock.Object);

			// Act
			var result = sut.UpdateCardBalance(initialCard.Id, updatedCard);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(updatedCard.Balance, result.Balance);
			repositoryMock.Verify(c => c.UpdateCardBalance(initialCard.Id, updatedCard), Times.Once);
		}

		[TestMethod]
		public void ThrowException_When_UpdateFails()
		{
			// Arrange
			var initialCard = TestHelper.GetTestCard();
			var updatedCard = TestHelper.GetTestCard();
			updatedCard.Balance = 2000m;

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(initialCard.Id))
				.Returns(initialCard);
			repositoryMock
				.Setup(c => c.UpdateCardBalance(It.IsAny<int>(), It.IsAny<Card>()))
				.Throws(new Exception("Update failed")); // Simulate update failure

			var sut = new CardService(repositoryMock.Object);

			// Act & Assert
			Assert.ThrowsException<Exception>(() => sut.UpdateCardBalance(initialCard.Id, updatedCard));
		}
	}
}