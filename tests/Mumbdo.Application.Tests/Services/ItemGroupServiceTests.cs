using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Jwt;
using Mumbdo.Application.Services;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Application.Tests.Services
{
    public class ItemGroupServiceTests
    {
        private AutoMocker _mocker;
        private Guid _userId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public async Task CreateAsync_Always_SavesToDb()
        {
            //Arrange
            var sut = CreateSut();
            var group = new Mock<IItemGroupEntity>().Object;
            var currentUser = new CurrentUser(Guid.NewGuid(), "test@test.com", "user");
            var dto = new CreateItemGroupDto("test", "test description", "http://myimage.com/image.png");
            _mocker.GetMock<ICurrentUserService>()
                .Setup(o => o.GetCurrentUser())
                .Returns(currentUser);

            _mocker.GetMock<IItemGroupAggregate>()
                .Setup(o => o.Create(currentUser.Id, dto.Name, dto.Description, dto.Image))
                .Returns(group);

            //Act
            await sut.CreateAsync(dto);

            //Assert
            _mocker.GetMock<IItemGroupRepository>()
                .Verify(o => o.CreateAsync(group));
            
        }

        [Test]
        public async Task GetAllForUserAsync_NoGroups_ReturnsEmpty()
        {
            //Arrange
            var sut = CreateSut();

            _mocker.GetMock<IItemGroupRepository>()
                .Setup(o => o.GetGroupsForUserAsync(_userId))
                .ReturnsAsync(new List<IItemGroupEntity>());
            
            SetupCurrentUser();
            
            //Act
            var result = await sut.GetAllForUserAsync();

            //Assert
            result.ShouldBeEmpty();
        }

        [Test]
        public async Task GetAllForUserAsync_SingleGroupNoTasks_ReturnsGroupDtoNoTasks()
        {
            //Arrange
            var sut = CreateSut();
            SetupCurrentUser();
            var name = "name";
            var description = "description";
            var userId = _userId;
            var image = "image";
            var id = Guid.NewGuid();
            var group = new Mock<IItemGroupEntity>();
            group.SetupGet(o => o.Name).Returns(name);
            group.SetupGet(o => o.Description).Returns(description);
            group.SetupGet(o => o.UserId).Returns(userId);
            group.SetupGet(o => o.Id).Returns(id);
            group.SetupGet(o => o.ImageUri).Returns(image);
            var groups = new List<IItemGroupEntity>() {group.Object};

            _mocker.GetMock<IItemGroupRepository>()
                .Setup(o => o.GetGroupsForUserAsync(_userId))
                .ReturnsAsync(groups);
            
            //Act
            var result = await sut.GetAllForUserAsync();
            
            //Assert
            result.Count().ShouldBe(1);
            var groupDto = result.First();
            groupDto.Id.ShouldBe(id);
            groupDto.Name.ShouldBe(name);
            groupDto.Description.ShouldBe(description);
            groupDto.Image.ShouldBe(image);
            groupDto.Tasks.ShouldBeEmpty();
        }
        
        
        [Test]
        public async Task GetAllForUserAsync_SingleGroupSingleTasks_ReturnsGroupDtoSingleTask()
        {
            //Arrange
            var sut = CreateSut();
            SetupCurrentUser();
            var name = "name";
            var userId = _userId;
            var id = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var group = new Mock<IItemGroupEntity>();
            var isComplete = true;
            DateTime? deadline = null;
            DateTime created = DateTime.Now;
            var task = new Mock<ITaskEntity>();
            task.SetupGet(o => o.Id).Returns(id);
            task.SetupGet(o => o.Name).Returns(name);
            task.SetupGet(o => o.UserId).Returns(userId);
            task.SetupGet(o => o.GroupId).Returns(groupId);
            task.SetupGet(o => o.Created).Returns(created);
            task.SetupGet(o => o.Deadline).Returns(deadline);
            task.SetupGet(o => o.IsComplete).Returns(isComplete);
            var groups = new List<IItemGroupEntity>() {group.Object};

            _mocker.GetMock<IItemGroupRepository>()
                .Setup(o => o.GetGroupsForUserAsync(_userId))
                .ReturnsAsync(groups);

            _mocker.GetMock<ITaskRepository>()
                .Setup(o => o.GetUnCompleteInGroupAsync(group.Object.Id, true))
                .ReturnsAsync(new List<ITaskEntity> {task.Object});
            
            //Act
            var result = await sut.GetAllForUserAsync();
            
            //Assert
            result.Count().ShouldBe(1);
            result.First().Tasks.Count().ShouldBe(1);
            var taskDto = result.First().Tasks.First();
            taskDto.Created.ShouldBe(created);
            taskDto.Id.ShouldBe(id);
            taskDto.Name.ShouldBe(name);
            taskDto.Deadline.ShouldBeNull();
            taskDto.IsComplete.ShouldBe(isComplete);
        }

        private void SetupCurrentUser()
        {
            var user = new CurrentUser(_userId, "email", "user");
            _mocker.GetMock<ICurrentUserService>()
                .Setup(o => o.GetCurrentUser())
                .Returns(user);
        }

        private IItemGroupService CreateSut() => _mocker.CreateInstance<ItemGroupService>();
    }
}