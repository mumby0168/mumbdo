using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Moq;
using Moq.AutoMock;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Common;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;
using Mumbdo.Web.Proxies;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Proxies
{
    public class TaskProxyTests : ProxyBaseTest
    {
        public static TaskDto DefaultTaskDto => new TaskDto(Guid.NewGuid(), "name", DateTime.Now, true, null, null);
        
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

        [Test]
        public async Task UpdateAsync_Always_UpdatesTask()
        {
            //Arrange
            var sut = CreateSut();
            SetupAuthentication();
            
            var id = Guid.NewGuid();
            var name = "name";
            var isComplete = true;
            var gId = Guid.NewGuid();
            var deadline = DateTime.Now.AddDays(2);

            var response = new Mock<IHttpResponse<TaskDto>>();
            
            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.PutAsync<UpdateTaskDto, TaskDto>(TaskUrls.UpdateTaskUrl, It.Is<UpdateTaskDto>(d => d.Id == id &&
                    d.Name == name &&
                    d.IsComplete == isComplete &&
                    d.Deadline == deadline &&
                    d.GroupId == gId)))
                .ReturnsAsync(response.Object);

            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(DefaultTaskDto);
            
            //Act
            var dto = await sut.UpdateAsync(id, name, isComplete, gId, deadline);

            //Assert
            dto.ShouldNotBeNull();
            VerifyAuthorisation();
        }

        [Test]
        public async Task DeleteAsync_Always_DeletesTask()
        {
            //Arrange
            var sut = CreateSut();
            SetupAuthentication();
            
            var id = Guid.NewGuid();

            var response = new Mock<IHttpResponse>();

            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.DeleteAsync(TaskUrls.DeleteTaskUrl(id)))
                .ReturnsAsync(response.Object);
            
            //Act
            await sut.DeleteAsync(id);

            //Assert
            VerifyAuthorisation();
            Mocker.GetMock<IProxyHelper>()
                .Verify(o => o.ProcessResponseAsync(response.Object, sut));
        }

        [Test]
        public async Task GetUngroupedTasksAsync_Always_GetsTasks()
        {
            //Arrange
            var sut = CreateSut();
            SetupAuthentication();
            var tasks = new List<TaskDto>();

            var response = new Mock<IHttpResponse<IEnumerable<TaskDto>>>();

            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.GetAsync<IEnumerable<TaskDto>>(TaskUrls.UngroupedTasksUrl(false)))
                .ReturnsAsync(response.Object);

            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(tasks);

            //Act
            var result = await sut.GetUngroupedTasksAsync(false);
            
            //Assert
            result.ShouldBeEmpty();
        }

        private TaskProxy CreateSut() => Mocker.CreateInstance<TaskProxy>();

        
    }
}