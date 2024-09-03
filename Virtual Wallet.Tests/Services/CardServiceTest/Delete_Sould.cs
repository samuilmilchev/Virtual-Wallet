using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Exceptions;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class Delete_Should
	{
		[TestMethod]
		public void DeleteCard_When_UserIsAuthorized()
		{
			// Arrange
			Card cardToDelete = TestHelper.GetTestCard();
			User user = new User { Username = "Justine Fox" };

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(cardToDelete.Id))
				.Returns(cardToDelete);

			repositoryMock
				.Setup(c => c.Delete(cardToDelete.Id))
				.Returns(true);

			var sut = new CardService(repositoryMock.Object);

			// Act
			bool result = sut.Delete(cardToDelete.Id, user);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		[ExpectedException(typeof(NotAuthorizedException))]
		public void ThrowNotAuthorizedException_When_UserIsNotAuthorized()
		{
			// Arrange
			Card cardToDelete = TestHelper.GetTestCard();
			User user = new User { Username = "DifferentUser" };

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(cardToDelete.Id))
				.Returns(cardToDelete);

			var sut = new CardService(repositoryMock.Object);

			// Act
			sut.Delete(cardToDelete.Id, user);
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void ThrowEntityNotFoundException_When_CardDoesNotExist()
		{
			// Arrange
			User user = new User { Username = "Justine Fox" };

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(It.IsAny<int>()))
				.Returns((Card)null);

			var sut = new CardService(repositoryMock.Object);

			// Act
			sut.Delete(1, user);
		}
	}
}