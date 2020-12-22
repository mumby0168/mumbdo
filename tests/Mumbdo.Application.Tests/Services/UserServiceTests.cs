using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Services;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Domain.ValueObjects;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using NUnit.Framework;

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

        private IUserService CreateSut() => _mocker.CreateInstance<UserService>();
    }
}