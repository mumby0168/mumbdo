using Moq.AutoMock;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Domain.ValueObjects;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Domain.Tests.Aggregates
{
    public class UserAggregateTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void Create_InvalidEmail_ThrowsEmailInvalid()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            //Assert
            Assert.Throws<EmailInvalidException>(
                () => sut.Create("invalidemail.234.outlook.com", new Password("", "", "")));
        }

        [Test]
        public void Create_ValidEmailAddress_CreatesUser()
        {
            //Arrange
            var sut = CreateSut();
            var email = "billy.mumby@outllook.com";
            var pass = new Password("", "", "");

            //Act
            var user = sut.Create(email, pass);

            //Assert
            user.Email.ShouldBe(email);
            user.Password.ShouldBe(pass);
        }
        

        private IUserAggregate CreateSut() => _mocker.CreateInstance<UserAggregate>();
    }
}