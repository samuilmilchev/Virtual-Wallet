using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Exceptions;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
	[TestClass]
	public class Create_Should
	{
		[TestMethod]
		public void CreateNewCard_When_CardNumberIsUnique()
		{
			// Arrange
			Card newCard = TestHelper.GetTestCard();
			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetByCardNumber(newCard.CardNumber))
				.Returns((Card)null);

			repositoryMock
				.Setup(c => c.Create(newCard))
				.Returns(newCard);

			var sut = new CardService(repositoryMock.Object);

			// Act
			Card createdCard = sut.Create(newCard);

			// Assert
			Assert.AreEqual(newCard, createdCard);
		}

		[TestMethod]
		[ExpectedException(typeof(DuplicateEntityException))]
		public void ThrowDuplicateEntityException_When_CardNumberAlreadyExists()
		{
			// Arrange
			Card existingCard = TestHelper.GetTestCard();
			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup(c => c.GetByCardNumber(existingCard.CardNumber))
				.Returns(existingCard);

			var sut = new CardService(repositoryMock.Object);

			// Act
			sut.Create(existingCard);
		}
	}
}