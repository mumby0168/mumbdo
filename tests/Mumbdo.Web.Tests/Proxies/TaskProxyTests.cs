using System;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;
using Mumbdo.Web.Proxies;
using NUnit.Framework;

namespace Mumbdo.Web.Tests.Proxies
{
    public class TaskProxyTests : ProxyBaseTest
    {

        [Test]
        public async Task CreateAsync_Always_CreatesTask()
        {
            //Arrange
            var sut = CreateSut();
            SetupAuthentication();
            var name = "name";
            var response = new Mock<IHttpResponse<TaskDto>>();
            
            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.PostAsync<CreateTaskDto, TaskDto>(TaskUrls.CreateTaskUrl,
                    It.Is<CreateTaskDto>(dto => dto.Name == name)))
                .ReturnsAsync(response.Object);
            
            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(new TaskDto(Guid.NewGuid(), "", DateTime.Now, false, Guid.Empty, DateTime.Now));

            //Act
            await sut.CreateAsync(name);

            //Assert
            VerifyAuthorisation();
            Mocker.GetMock<IProxyHelper>()
                .Verify(o => o.ProcessResponseAsync(response.Object, sut));

        }

        private TaskProxy CreateSut() => Mocker.CreateInstance<TaskProxy>();

        
    }
}