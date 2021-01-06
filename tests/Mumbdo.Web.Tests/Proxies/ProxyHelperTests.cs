using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Web.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Proxies;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Proxies
{
    public class ProxyHelperTests
    {
        private AutoMocker _mocker;
        private Mock<ProxyBase> _base;
        private HttpResponseMessage _message;
        private object _data;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _base = new Mock<ProxyBase>();
            _message = new HttpResponseMessage();
            _data = new object();
        }

        [Test]
        public async Task ProcessResponseAsync_RefusedConnection_SetRefusedError()
        {
            //Arrange
            var sut = CreateSut();
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse(null, true), _base.Object);
            
            //Assert
            _base.Verify(o => o.SetConnectionRefusedError());
        }
        
        [Test]
        public async Task ProcessResponseAsync_OkStatusCode_SetsNoError()
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = HttpStatusCode.OK;

            //Act
            await sut.ProcessResponseAsync(new HttpResponse(_message), _base.Object);
            
            //Assert
            VerifyNoErrorSet();
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        public async Task ProcessResponseAsync_FailedStatusCode_SetFailureMessage(HttpStatusCode code)
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = code;
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse(_message), _base.Object);
            
            //Assert
            _base.Verify(o => o.ProcessErrorAsync(_message));
        }
        
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.Unused)]
        public async Task ProcessResponseAsync_UnHandledStatus_SetRefusedError(HttpStatusCode code)
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = code;
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse(_message), _base.Object);
            
            //Assert
            _base.Verify(o => o.SetConnectionRefusedError());
        }
        
        [Test]
        public async Task ProcessResponseAsyncT_RefusedConnection_SetRefusedError()
        {
            //Arrange
            var sut = CreateSut();
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse<object>(null, _data, true), _base.Object);

            //Assert
            _base.Verify(o => o.SetConnectionRefusedError());
        }
        
        [Test]
        public async Task ProcessResponseAsyncT_OkStatusCode_SetsNoError()
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = HttpStatusCode.OK;

            //Act
            var result = await sut.ProcessResponseAsync(new HttpResponse<object>(_message, _data), _base.Object);
            
            //Assert
            VerifyNoErrorSet();
            result.ShouldBe(_data);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        public async Task ProcessResponseAsyncT_FailedStatusCode_SetFailureMessage(HttpStatusCode code)
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = code;
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse<object>(_message, _data), _base.Object);
            
            //Assert
            _base.Verify(o => o.ProcessErrorAsync(_message));
        }
        
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.Unused)]
        public async Task ProcessResponseAsyncT_UnHandledStatus_SetRefusedError(HttpStatusCode code)
        {
            //Arrange
            var sut = CreateSut();
            _message.StatusCode = code;
            
            //Act
            await sut.ProcessResponseAsync(new HttpResponse<object>(_message, _data), _base.Object);
            
            //Assert
            _base.Verify(o => o.SetConnectionRefusedError());
        }

        private void VerifyNoErrorSet()
        {
            _base.Verify(o => o.SetErrorMessage(It.IsAny<string>()), Times.Never);
            _base.Verify(o => o.ProcessErrorAsync(It.IsAny<HttpResponseMessage>()), Times.Never);
            _base.Verify(o => o.SetConnectionRefusedError(), Times.Never);
        }
        

        private IProxyHelper CreateSut() => _mocker.CreateInstance<ProxyHelper>();
    }
}