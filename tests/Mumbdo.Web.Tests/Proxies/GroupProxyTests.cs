using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Common;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Proxies;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Web.Tests.Proxies
{
    public class GroupProxyTests : ProxyBaseTest
    {
        [TestCase(true)] 
        [TestCase(false)] 
        public async Task GetGroupsAsync_Always_HandlesResult(bool includeTasks)
        {
            //Arrange
            var sut = CreateSut();
            var response = new Mock<IHttpResponse<IEnumerable<ItemGroupDto>>>();
            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.GetAsync<IEnumerable<ItemGroupDto>>(GroupUrls.GetGroupsUrl(includeTasks)))
                .ReturnsAsync(response.Object);

            var groupDto = new ItemGroupDto(Guid.NewGuid(), "", "", "", new List<TaskDto>());

                Mocker.GetMock<IProxyHelper>()
                    .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                    .ReturnsAsync(new[] {groupDto});

            //Act
            var result = await sut.GetGroupsAsync(includeTasks);

            //Assert
            result.Count().ShouldBe(1);
        }
        
        [Test]
        public async Task CreateAsync_Always_Creates()
        {
            //Arrange
            var sut = CreateSut();
            var response = new Mock<IHttpResponse>();
            var dto = new CreateItemGroupDto("", "", "");
            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.PostAsync<CreateItemGroupDto>(GroupUrls.CreateGroupUrl, dto))
                .ReturnsAsync(response.Object);

            //Act
            await sut.CreateAsync("", "", "");

            //Assert
            Mocker.GetMock<IProxyHelper>()
                .Verify(o => o.ProcessResponseAsync(response.Object, sut));
        }

        [Test]
        public async Task GetAsync_Always_Gets()
        {
            //Arrange
            var sut = CreateSut();
            var id = Guid.NewGuid();
            var response = new Mock<IHttpResponse<ItemGroupDto>>();
            Mocker.GetMock<IHttpClient>()
                .Setup(o => o.GetAsync<ItemGroupDto>(GroupUrls.GetGroupUrl(id, true)))
                .ReturnsAsync(response.Object);

            var groupDto = new ItemGroupDto(id, "", "", "", new List<TaskDto>());

            Mocker.GetMock<IProxyHelper>()
                .Setup(o => o.ProcessResponseAsync(response.Object, sut))
                .ReturnsAsync(groupDto);

            //Act
            var result = await sut.GetAsync(id, true);

            //Assert
            result.Id.ShouldBe(id);
        }
        
        private GroupProxy CreateSut() => Mocker.CreateInstance<GroupProxy>();
    }
}