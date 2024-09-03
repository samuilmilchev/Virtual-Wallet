using Moq;
using Virtual_Wallet.Services;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;

namespace Virtual_Wallet.Tests.Services.CardServiceTest
{
    [TestClass]
	public class GetById_Should
	{
		[TestMethod]
		public void ReturnCorrectCard_When_ParametersAreValid()
		{
			//Arrange
			Card expectedCard = TestHelper.GetTestCard();

			var repositoryMock = new Mock<ICardRepository>();
			repositoryMock
				.Setup( c => c.GetById(1))
				.Returns(expectedCard);

			var sut = new CardService(repositoryMock.Object);

			//Act
			Card actualCard = sut.GetById(expectedCard.Id);

			//Assert
			Assert.AreEqual(expectedCard, actualCard);
		}
	}
}
