using System;
using System.Diagnostics.SymbolStore;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Jwt;
using Mumbdo.Application.Services;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Domain.ValueObjects;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Application.Tests.Services
{
    public class UserServiceTests
    {
        private AutoMocker _mocker;
        private const string InUseEmail = "iaminuse@outlook.com";
        private const string Email = "validemailaddress@outlook.com";
        private const string Password = "-ThisIsAStrongPassword123-£";
        public const string WeakPassword = "pass123";

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void CreateAsync_InUseEmail_ThrowsInUseEmailException()
        {
            //Arrange
            var sut = CreateSut();
            _mocker.GetMock<IUserRepository>()
                .Setup(o => o.IsEmailInUseAsync(InUseEmail))
                .Returns(Task.FromResult(true));

            //Act
            //Assert
            Assert.ThrowsAsync<EmailInUseException>(() => sut.CreateAsync(new CreateUserDto(InUseEmail, "")));
        }

        [Test]
        public void CreateAsync_WeakPassword_ThrowsPasswordToWeakException()
        {
            //Arrange
            var sut = CreateSut();
            _mocker.GetMock<IPasswordService>()
                .Setup(o => o.IsStrongPassword(WeakPassword))
                .Returns(false);
            //Act
            //Assert
            Assert.ThrowsAsync<PasswordToWeakException>(() => sut.CreateAsync(new CreateUserDto(Email, WeakPassword)));
        }

        [Test]
        public async Task CreateAsync_ValidUser_CreatesUser()
        {
            //Arrange
            var sut = CreateSut();
            var pass = new Password("", "", "");
            var user = new Mock<IUser>();
            _mocker.GetMock<IPasswordService>()
                .Setup(o => o.IsStrongPassword(Password))
                .Returns(true);
            _mocker.GetMock<IPasswordService>()
                .Setup(o => o.HashPassword(Password, It.IsAny<string>(), It.IsAny<int>()))
                .Returns(pass);
            _mocker.GetMock<IUserAggregate>()
                .Setup(o => o.Create(Email, It.IsAny<Password>()))
                .Returns(user.Object);
            
            //Act
            await sut.CreateAsync(new CreateUserDto(Email, Password));

            //Assert
            _mocker.GetMock<IUserRepository>().Verify(o => o.SaveAsync(user.Object));
        }

        [Test]
        public void SignInAsync_InvalidEmailAddress_ThrowsInvalidCredentials()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            //Assert
            Assert.ThrowsAsync<InvalidUserCredentialsException>(() =>
                sut.SignInAsync(new SignInDto(Email, Password)));
        }

        [Test]
        public void SignInAsync_WrongPassword_ThrowsInvalidCredentials()
        {
            //Arrange
            var sut = CreateSut();
            var user = new Mock<IUser>();
            _mocker.GetMock<IUserRepository>()
                .Setup(o => o.GetByEmailAsync(Email))
                .ReturnsAsync(user.Object);

            //Act
            //Assert
            Assert.ThrowsAsync<InvalidUserCredentialsException>(() =>
                sut.SignInAsync(new SignInDto(Email, Password)));
            _mocker.GetMock<IPasswordService>()
                .Verify(o => o.CheckPassword(Password, user.Object.Password));
        }

        [Test]
        public async Task SignInAsync_ValidCredentials_ReturnsAndSavesTokenData()
        {
            //Arrange
            var sut = CreateSut();
            var user = new Mock<IUser>();
            _mocker.GetMock<IUserRepository>()
                .Setup(o => o.GetByEmailAsync(Email))
                .ReturnsAsync(user.Object);
            var token = "a super secret token";
            var refresh = "a super secret refresh token";
            _mocker.GetMock<ITokenService>().Setup(o => o.CreateToken(user.Object)).Returns(token);
            _mocker.GetMock<IPasswordService>().Setup(o => o.GenerateSalt(32)).Returns(refresh);
            _mocker.GetMock<IPasswordService>().Setup(o => o.CheckPassword(Password, user.Object.Password))
                .Returns(true);

            //Act
            var result = await sut.SignInAsync(new SignInDto(Email, Password));
            result.Refresh.ShouldBe(refresh);
            result.Token.ShouldBe(token);
            _mocker.GetMock<IRefreshTokenRepository>().Verify(o => o.SaveTokenAsync(user.Object.Id, refresh, It.IsAny<DateTime>()));
        }

        [Test]
        public void RefreshAsync_InvalidEmail_ThrowsInvalidToken()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            //Assert
            Assert.ThrowsAsync<InvalidRefreshTokenException>(() => sut.RefreshAsync("token", "invalid"));
        }

        [Test]
        public void RefreshAsync_InvalidRefreshToken_ThrowsInvalidRefreshToken()
        {
            //Arrange
            var sut = CreateSut();
            var email = "test@test.com";
            var user = new Mock<IUser>();
            _mocker.GetMock<IUserRepository>()
                .Setup(o => o.GetByEmailAsync(email))
                .ReturnsAsync(user.Object);
            
            //Act
            //Assert
            Assert.ThrowsAsync<InvalidRefreshTokenException>(() => sut.RefreshAsync("token", email));
        }

        [Test]
        public async Task RefreshAsync_ValidToken_RefreshesToken()
        {
            //Arrange
            var sut = CreateSut();
            var email = "test@test.com";
            var user = new Mock<IUser>();
            _mocker.GetMock<IUserRepository>()
                .Setup(o => o.GetByEmailAsync(email))
                .ReturnsAsync(user.Object);
            var refreshToken = "refresh-token";
            var token = "token";
            _mocker.GetMock<ITokenService>().Setup(o => o.CreateToken(user.Object)).Returns(token);
            

            _mocker.GetMock<IRefreshTokenRepository>()
                .Setup(o => o.IsTokenValid(user.Object.Id, refreshToken))
                .ReturnsAsync(true);

            //Act
            var newToken = await sut.RefreshAsync(refreshToken, email);

            //Assert
            newToken.Refresh.ShouldBe(refreshToken);
            newToken.Token.ShouldBe(token);
        }

        private IUserService CreateSut() => _mocker.CreateInstance<UserService>();
    }
}