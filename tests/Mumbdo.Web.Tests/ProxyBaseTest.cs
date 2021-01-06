using System.Net.Http;
using Moq;
using Moq.AutoMock;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using NUnit.Framework;

namespace Mumbdo.Web.Tests
{
    public abstract class ProxyBaseTest
    {
        protected AutoMocker Mocker;
        protected const string Token = "test-token";
        protected HttpResponseMessage HttpResponse;

        [SetUp]
        protected virtual void Setup()
        {
            Mocker = new AutoMocker();
            HttpResponse = new HttpResponseMessage();
        }

        protected virtual void VerifyAuthorisation()
        {
            Mocker.GetMock<IHttpClient>()
                .Verify(o => o.AddBearerToken(Token));
        }

        protected virtual void SetupAuthentication()
        {
            var auth = Mocker.GetMock<IAuthenticationService>();
            auth
                .Setup(o => o.IsUserSignedInAsync())
                .ReturnsAsync(true);
            
            auth
                .SetupGet(o => o.Token)
                .Returns(Token);

        }
    }
}