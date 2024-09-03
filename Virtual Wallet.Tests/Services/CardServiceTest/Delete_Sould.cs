using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class Delete_Should
	{
		[TestMethod]
		public void DeleteCard_When_UserIsAuthorized()
		{
			// Arrange
			var user = new User { Username = "existing_user" }; // Assume this user already exists
			var cardToDelete = TestHelper.GetTestCard();
			cardToDelete.User = user; // Ensure the card is associated with the correct user

			// Mock repository
			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(cardToDelete.Id))
				.Returns(cardToDelete); // Return the card to be deleted

			repositoryMock
				.Setup(c => c.Delete(cardToDelete.Id))
				.Returns(true); // Simulate successful deletion

			var sut = new CardService(repositoryMock.Object);

			// Act
			bool result = sut.Delete(cardToDelete.Id, user);

			// Assert
			Assert.IsTrue(result, "The card should be deleted successfully.");
		}

		[TestMethod]
		public void ThrowNotAuthorizedException_When_UserIsNotAuthorized()
		{
			// Arrange
			var cardToDelete = TestHelper.GetTestCard();
			var unauthorizedUser = new User { Username = "UnauthorizedUser" }; // Simulate unauthorized user

			// Set the User of cardToDelete to a user different from unauthorizedUser
			cardToDelete.User = new User { Username = "AuthorizedUser" }; // Ensure this is different from unauthorizedUser

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(cardToDelete.Id))
				.Returns(cardToDelete); // Return the card with the correct User

			var sut = new CardService(repositoryMock.Object);

			// Act & Assert
			Assert.ThrowsException<NotAuthorizedException>(() => sut.Delete(cardToDelete.Id, unauthorizedUser));
		}

		[TestMethod]
		[ExpectedException(typeof(EntityNotFoundException))]
		public void ThrowEntityNotFoundException_When_CardDoesNotExist()
		{
			// Arrange
			var nonExistentCardId = 999;
			var user = new User { Username = "SomeUser" }; // Simulate authorized user

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetById(nonExistentCardId))
				.Throws(new EntityNotFoundException($"Card with id={nonExistentCardId} doesn't exist.")); // Simulate card not found

			var sut = new CardService(repositoryMock.Object);

			// Act
			sut.Delete(nonExistentCardId, user);
		}
	}
}