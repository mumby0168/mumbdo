using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Moq;
using Moq.AutoMock;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Authentication;
using Mumbdo.Web.Common;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Authentication
{
    public class AuthenticationProxyTests
    {
        private AutoMocker _mocker;
        private const string Email = "test@test.com";
        private const string Password = "Password123";

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o => o.ConnectionRefusedErrorMessage)
                .Returns(CommonNames.ConnectionRefusedError);
        }

        [Test]
        public async Task SignInAsync_InvalidCredentials_ReturnsNullAndError()
        {
            //Arrange
            var sut = CreateSut();
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Unauthorized;
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.PostDataAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,
                        It.Is<SignInDto>(o => o.Email == Email && o.Password == Password)))
                .ReturnsAsync(new Tuple<HttpResponseMessage, JwtTokenDto>(response, null));
            

            //Act
            var result = await sut.SignInAsync(Email, Password);

            //Assert
            result.ShouldBeNull();
            sut.ErrorMessage.ShouldBe(CommonNames.ConnectionRefusedError);
        }

        [Test]
        public async Task SignInAsync_RefusedConnection_ReturnsNullAndError()
        {
            //Arrange
            var sut = CreateSut();
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.PostDataAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,It.IsAny<SignInDto>()))
                .Throws(new Exception());
            
            //Act
            var result = await sut.SignInAsync(Email, Password);
            result.ShouldBeNull();
            sut.ErrorMessage.ShouldBe(CommonNames.ConnectionRefusedError);
        }
        
        
        [Test]
        public async Task SignInAsync_ValidCredentials_ReturnsToken()
        {
            //Arrange
            var sut = CreateSut();
            var token = new JwtTokenDto("", "");
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.PostDataAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,
                        It.Is<SignInDto>(o => o.Email == Email && o.Password == Password)))
                .ReturnsAsync(new Tuple<HttpResponseMessage, JwtTokenDto>(null, token));
            
            //Act
            var result = await sut.SignInAsync(Email, Password);

            //Assert
            result.ShouldBe(token);
        }
        
        [Test]
        public async Task RefreshAsync_RefusedConnection_ReturnsNullAndError()
        {
            //Arrange
            var email = "email";
            var token = "token";
            var sut = CreateSut();
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(token, email)))
                .Throws(new Exception());
            
            //Act
            var result = await sut.RefreshAsync(token, email);
            result.ShouldBeNull();
            sut.ErrorMessage.ShouldBe(CommonNames.ConnectionRefusedError);
        }
        
        [Test]
        public async Task RefreshAsync_InvalidRefresh_ReturnsNullAndError()
        {
            //Arrange
            var email = "email";
            var token = "token";
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Unauthorized;
            var sut = CreateSut();
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(token, email)))
                .ReturnsAsync(new Tuple<HttpResponseMessage, JwtTokenDto>(response, null));

            //Act
            var result = await sut.RefreshAsync(token, email);
            result.ShouldBeNull();
            sut.ErrorMessage.ShouldBe(CommonNames.ConnectionRefusedError);
        }
        
        [Test]
        public async Task RefreshAsync_ValidToken_ReturnsNewToken()
        {
            //Arrange
            var email = "email";
            var token = "token";
            var tokenResponse = new JwtTokenDto("", "");
            var sut = CreateSut();
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o =>
                    o.GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(token, email)))
                .ReturnsAsync(new Tuple<HttpResponseMessage, JwtTokenDto>(null, tokenResponse));

            //Act
            var result = await sut.RefreshAsync(token, email);

            //Assert
            result.ShouldBe(tokenResponse);
        }

        [Test]
        public async Task SignUpAsync_BadRequest_SetsErrorMessage()
        {
            //Arrange
            var sut = CreateSut();
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.BadRequest;
            var error = "test error message";
            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o => o.ParseErrorAsync(response))
                .ReturnsAsync(new MumbdoErrorDto("code", error));

            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o => o.PostAsync(AuthenticationUrls.SignUp, It.IsAny<CreateUserDto>()))
                .ReturnsAsync(response);

            //Act
            await sut.SignUpAsync("email", "pass");

            //Assert
            sut.ErrorMessage.ShouldBe(error);
        }
        
        [Test]
        public async Task SignUpAsync_NoConnection_SetsErrorMessage()
        {
            //Arrange
            var sut = CreateSut();

            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o => o.PostAsync(AuthenticationUrls.SignUp, It.IsAny<CreateUserDto>()))
                .ThrowsAsync(new Exception());

            //Act
            await sut.SignUpAsync("email", "pass");

            //Assert
            sut.ErrorMessage.ShouldBe(CommonNames.ConnectionRefusedError);
        }
        
        
        [Test]
        public async Task SignUpAsync_ValidDetails_NoException()
        {
            //Arrange
            var sut = CreateSut();
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            var error = "test error message";

            _mocker.GetMock<IMumbdoHttpClient>()
                .Setup(o => o.PostAsync(AuthenticationUrls.SignUp, It.IsAny<CreateUserDto>()))
                .ReturnsAsync(response);

            //Act
            //Assert
            await sut.SignUpAsync("email", "pass");

        }
        

        private IAuthenticationProxy CreateSut() => _mocker.CreateInstance<AuthenticationProxy>();
    }
}