using System;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Authentication;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Managers;
using Mumbdo.Web.Tests.Tools;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Authentication
{
    public class AuthenticationServiceTests
    {
        private AutoMocker _mocker;
        private bool _hasAuthStateChanged = false;

        private const string Token =
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJiNGRmYmZlOC04ZWFiLTQyZjktYjNiZS04MTBkYTJlNGYyM2EiLCJlbWFpbCI6Indqbm0ubXVtYnkxQGdtYWlsLmNvbSIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNjA4NzQ0NDkxLCJleHAiOjE2MDg3NTUyOTEsImlhdCI6MTYwODc0NDQ5MX0.q0Vz10NNvtu7qwzbXsNE3JLph7uDxzN2t_NAnrHzmp9nlVKu6MMgtRbL01YwUugFkxBEDsG2O4EiVjo6eS7ErQ";

        private const string Refresh = "refresh";

        private readonly JwtTokenDto _validToken = new JwtTokenDto(Token, Refresh);
        private SignedInUser _user;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _mocker.GetMock<IUserContext>().SetupAllProperties();
        }

        [Test]
        public async Task SignInAsync_Always_SignsUserIn()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);

            //Act
            await sut.SignInAsync(_validToken);
            
            //Assert
            _hasAuthStateChanged.ShouldBeTrue();
            _mocker.GetMock<ITokenManager>().Verify(o => o.SaveTokenAsync(_validToken));
            sut.User.ShouldNotBeNull();
        }

        [Test]
        public async Task SignOutAsync_Always_SignsOutUser()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);
            await sut.SignInAsync(_validToken);
            _hasAuthStateChanged = false;

            //Act
            await sut.SignOutAsync();
            
            //Assert
            _hasAuthStateChanged.ShouldBeTrue();
            _mocker.GetMock<ITokenManager>().Verify(o => o.RemoveTokenAsync());
            sut.User.ShouldBeNull();
        }

        [Test]
        public async Task IsUserSignedInAsync_SignedInUserWithValidToken_ReturnsTrue()
        {
            //Arrange
            var sut = CreateSut();
            _user = new SignedInUser() {Expiry = DateTime.Now.AddHours(1)};
            _mocker.GetMock<IUserContext>().Object.SignedInUser = _user;

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            result.ShouldBeTrue();
        }
        
        [Test]
        public async Task IsUserSignedInAsync_SignedInUserWithInvalidToken_ReturnsFalseAndSignsOut()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);
            _user = new SignedInUser() {Expiry = DateTime.Now.AddHours(-1)};
            _mocker.GetMock<IUserContext>().Object.SignedInUser = _user;

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            result.ShouldBeFalse();
            _hasAuthStateChanged.ShouldBeTrue();
            sut.User.ShouldBeNull();
        }
        
        [Test]
        public async Task IsUserSignedInAsync_NoSignedInUserNoSavedToken_ReturnsFalse()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            _hasAuthStateChanged.ShouldBeFalse();
            result.ShouldBeFalse();
            _user.ShouldBeNull();
        }
        
        [Test]
        public async Task IsUserSignedInAsync_NoSignedInUserSavedNotExpired_ReturnsFalse()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);
            _mocker.GetMock<ITokenManager>().Setup(o => o.GetTokenAsync())
                .ReturnsAsync(JwtHelper.CreateFakeToken(Guid.NewGuid(), "", "", DateTime.Now.AddHours(1)));

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            result.ShouldBeTrue();
            _user.ShouldBeNull();
        }
        
        [Test]
        public async Task IsUserSignedInAsync_NoSignedInUserSavedExpiredAndRefreshFails_ReturnsFalse()
        {
            //Arrange
            var sut = CreateSut();
            SubscribeAuthChanged(sut);
            _mocker.GetMock<ITokenManager>().Setup(o => o.GetTokenAsync())
                .ReturnsAsync(JwtHelper.CreateFakeToken(Guid.NewGuid(), "", "", DateTime.Now.AddHours(-1)));

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            result.ShouldBeFalse();
            _user.ShouldBeNull();
        }
        
        [Test]
        public async Task IsUserSignedInAsync_NoSignedInUserSavedExpiredAndRefreshSucceeds_ReturnsTrue()
        {
            //Arrange
            var sut = CreateSut();
            var email = "test@test-email.com";
            var id = Guid.NewGuid();
            var role = "role";
            var refresh = Guid.NewGuid().ToString();
            SubscribeAuthChanged(sut);
            _mocker.GetMock<ITokenManager>().Setup(o => o.GetTokenAsync())
                .ReturnsAsync(JwtHelper.CreateFakeToken(id, email, role, DateTime.Now.AddHours(-1), refresh));

            _mocker.GetMock<IAuthenticationProxy>()
                .Setup(o => o.RefreshAsync(refresh, email))
                .ReturnsAsync(JwtHelper.CreateFakeToken(id, email, role, DateTime.Now.AddHours(-1)));

            //Act
            var result = await sut.IsUserSignedInAsync();

            //Assert
            result.ShouldBeTrue();
            sut.User.ShouldNotBeNull();
            sut.EmailAddress.ShouldBe(email);
            sut.Id.ShouldBe(id);
            sut.Role.ShouldBe(role);
        }

        private void SubscribeAuthChanged(IAuthenticationService sut) =>
            sut.AuthenticationStateUpdated += ((sender, args) => _hasAuthStateChanged = true);
        
        

        private IAuthenticationService CreateSut() => _mocker.CreateInstance<AuthenticationService>();
    }
}