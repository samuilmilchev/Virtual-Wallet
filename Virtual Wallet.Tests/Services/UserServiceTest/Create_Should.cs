using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Virtual_Wallet.Exceptions;
using Virtual_Wallet.Models.Entities;
using Virtual_Wallet.Repository.Contracts;
using Virtual_Wallet.Services;
using Virtual_Wallet.Services.Contracts;
using FluentAssertions;

namespace Virtual_Wallet.Tests.Services
{
    [TestClass]
    public class UsersServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IEmailService> _emailServiceMock;
        private UsersService _usersService;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _usersService = new UsersService(_userRepositoryMock.Object, _emailServiceMock.Object);
        }

        [TestMethod]
        public void Create_Should_Return_CreatedUser_When_ValidUserProvided()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                PhoneNumber = "1234567890"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserPhoneNumberExists(user.PhoneNumber)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserNameExists(user.Username)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.Create(It.IsAny<User>())).Returns(user);

            // Act
            var result = _usersService.Create(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.PhoneNumber, result.PhoneNumber);
            _userRepositoryMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_UserEmailExists()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "duplicate@example.com",
                PhoneNumber = "1234567890"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(true);

            // Act
            _usersService.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_UserPhoneNumberExists()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "testuser@example.com",
                PhoneNumber = "duplicate_phone"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserPhoneNumberExists(user.PhoneNumber)).Returns(true);

            // Act
            _usersService.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_UserNameExists()
        {
            // Arrange
            var user = new User
            {
                Username = "duplicate_username",
                Email = "testuser@example.com",
                PhoneNumber = "1234567890"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserPhoneNumberExists(user.PhoneNumber)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserNameExists(user.Username)).Returns(true);

            // Act
            _usersService.Create(user);
        }
        [TestMethod]
        public void GetById_Should_Return_User_When_UserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser", Email = "testuser@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);

            // Act
            var result = _usersService.GetById(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(user.Username, result.Username);
        }

        //[TestMethod]
        //public void GetByUsername_Should_Throw_EntityNotFoundException_When_UserDoesNotExist()
        //{
        //    // Arrange
        //    var username = "someuser";
        //    _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns((User)null);

        //    // Act & Assert
        //    var exception = Assert.ThrowsException<EntityNotFoundException>(() => _usersService.GetByUsername(username));
        //    Assert.AreEqual($"User with username {username} does not exist!", exception.Message);
        //}

        [TestMethod]
        public void GetByEmail_Should_Return_User_When_UserExists()
        {
            // Arrange
            var email = "testuser@example.com";
            var user = new User { Id = 1, Username = "testuser", Email = email };
            _userRepositoryMock.Setup(repo => repo.GetByEmail(email)).Returns(user);

            // Act
            var result = _usersService.GetByEmail(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        //[TestMethod]
        //public void GetByEmail_Should_Throw_EntityNotFoundException_When_UserDoesNotExist()
        //{
        //    // Arrange
        //    var email = "testuser@example.com";
        //    _userRepositoryMock.Setup(repo => repo.GetByEmail(email)).Returns((User)null);

        //    // Act & Assert
        //    var exception = Assert.ThrowsException<EntityNotFoundException>(() => _usersService.GetByEmail(email));
        //    Assert.AreEqual($"User with e-mail: {email} does not exist!", exception.Message);
        //}

        [TestMethod]
        public void GetByUsername_Should_Return_User_When_UserExists()
        {
            // Arrange
            var username = "testuser";
            var user = new User { Id = 1, Username = username, Email = "testuser@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns(user);

            // Act
            var result = _usersService.GetByUsername(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.Username);
        }

        //[TestMethod]
        //[ExpectedException(typeof(EntityNotFoundException))]
        //public void GetByUsername_Should_Throw_Exception_When_UserDoesNotExist()
        //{
        //    // Arrange
        //    var username = "someuser";
        //    _userRepositoryMock.Setup(repo => repo.GetByUsername(username)).Returns((User)null);

        //    // Act
        //    _usersService.GetByUsername(username);
        //}

        [TestMethod]
        public void Delete_Should_Delete_User_When_UserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser", Email = "testuser@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);

            // Act
            _usersService.Delete(userId, user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Delete(user.Id), Times.Once);
        }

        //[TestMethod]
        //public void Delete_Should_Throw_EntityNotFoundException_When_UserDoesNotExist()
        //{
        //    // Arrange
        //    var userId = 5;
        //    _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns((User)null);
        //    var user = new User { Id = userId }; // Ensure user is initialized properly

        //    // Act & Assert
        //    var exception = Assert.ThrowsException<EntityNotFoundException>(() => _usersService.Delete(userId, user));

        //    // Optionally, verify the exception message
        //    Assert.AreEqual($"User with ID {userId} does not exist!", exception.Message);
        //}

        [TestMethod]
        public void Update_Should_Update_User_When_UserExists()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser", Email = "testuser@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _userRepositoryMock.Setup(repo => repo.Update(userId, user)).Returns(user);

            // Act
            var result = _usersService.Update(userId, user);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual(user.Username, result.Username);
            _userRepositoryMock.Verify(repo => repo.Update(userId, user), Times.Once);
        }

        //[TestMethod]
        //public void Update_Should_Throw_EntityNotFoundException_When_UserDoesNotExist()
        //{
        //    // Arrange
        //    var user = new User { Id = 10, Username = "testuser", Email = "testuser@example.com" };
        //    _userRepositoryMock.Setup(repo => repo.GetById(user.Id)).Returns((User)null);

        //    // Act & Assert
        //    var exception = Assert.ThrowsException<EntityNotFoundException>(() => _usersService.Update(user.Id, user));

        //    // Optionally, verify the exception message
        //    Assert.AreEqual($"User with ID {user.Id} does not exist!", exception.Message);
        //}

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_Email_Already_Exists()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                Email = "existing@example.com",
                PhoneNumber = "1234567890"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(true);

            // Act
            _usersService.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_PhoneNumber_Already_Exists()
        {
            // Arrange
            var user = new User
            {
                Username = "newuser",
                Email = "newuser@example.com",
                PhoneNumber = "existing_phone"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserPhoneNumberExists(user.PhoneNumber)).Returns(true);

            // Act
            _usersService.Create(user);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateEntityException))]
        public void Create_Should_Throw_Exception_When_Username_Already_Exists()
        {
            // Arrange
            var user = new User
            {
                Username = "existing_username",
                Email = "newuser@example.com",
                PhoneNumber = "1234567890"
            };

            _userRepositoryMock.Setup(repo => repo.UserEmailExists(user.Email)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserPhoneNumberExists(user.PhoneNumber)).Returns(false);
            _userRepositoryMock.Setup(repo => repo.UserNameExists(user.Username)).Returns(true);

            // Act
            _usersService.Create(user);
        }
    }
}