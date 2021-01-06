using System.Threading.Tasks;
using Moq;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Authentication
{
    public class AuthenticationProxyTests : ProxyBaseTest
    {
        private const string Email = "test@test.com";
        private const string Password = "Password123";

        
        [Test]
        public async Task RefreshAsync_Always_RefreshesToken()
        {
            //Arrange
            var sut = CreateSut();
            var response = new Mock<IHttpResponse<JwtTokenDto>>();
            var tokenDto = new JwtTokenDto(Token, "");

            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.
                    GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(Token, Email)))
                .ReturnsAsync(response.Object);

            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(tokenDto);

            //Act
            await sut.RefreshAsync(Token, Email);

            //Assert
            Mocker.GetMock<IProxyHelper>()
                .Verify(o => o.ProcessResponseAsync(response.Object, sut));
        }

        [Test]
        public async Task SignUpAsync_Always_SignsUp()
        {
            //Arrange
            var sut = CreateSut();
            var dto = new CreateUserDto(Email, Password);
            var response = new Mock<IHttpResponse>();

            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.PostAsync<CreateUserDto>(AuthenticationUrls.SignUp,
                    It.Is<CreateUserDto>(dto => dto.Email == Email && dto.Password == Password)))
                .ReturnsAsync(response.Object);

            //Act
            await sut.SignUpAsync(Email, Password);

            //Assert
            Mocker.GetMock<IProxyHelper>()
                .Verify(o => o.ProcessResponseAsync(response.Object, sut));
        }

        [Test]
        public async Task SignInAsync_Always_SignsIn()
        {
            //Arrange
            var sut = CreateSut();
            var response = new Mock<IHttpResponse<JwtTokenDto>>();
            var token = new JwtTokenDto(Token, "refresh");

            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.PostAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,
                    It.Is<SignInDto>(dto => dto.Email == Email && dto.Password == Password)))
                .ReturnsAsync(response.Object);

            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(token);

            //Act
            var result = await sut.SignInAsync(Email, Password);

            //Assert
            result.ShouldBe(token);
        }


        private AuthenticationProxy CreateSut() => Mocker.CreateInstance<AuthenticationProxy>();
    }
}