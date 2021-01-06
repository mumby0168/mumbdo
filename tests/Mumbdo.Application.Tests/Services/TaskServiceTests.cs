using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Interfaces.Utilities;
using Mumbdo.Application.Services;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Application.Tests.Services
{
    public class TaskServiceTests
    {
        private AutoMocker _mocker;
        private Guid _userId;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void CreateAsync_InvalidGroupId_Throws()
        {
            //Arrange
            var sut = CreateSut();
            
            //Act
            //Assert
            Assert.ThrowsAsync<GroupIdInvalidException>(() =>
                sut.CreateAsync(new CreateTaskDto("", Guid.NewGuid(), null)));
        }

        [Test]
        public async Task CreateAsync_NoGroupId_CreatesTask()
        {
            //Arrange
            var sut = CreateSut();
            var dto = new CreateTaskDto("name", null, null);
            
            Tools.SetupCurrentUserService(_mocker);

            var task = new Mock<ITaskEntity>();
            task.SetupGet(o => o.Name).Returns(dto.Name);

            _mocker.GetMock<ITaskAggregate>()
                .Setup(o => o.Create(dto.Name, Tools.User.Id, null, null))
                .Returns(task.Object);
                            
            //Act
            var result = await sut.CreateAsync(dto);
            
            //Assert
            result.Name.ShouldBe(task.Object.Name);
            _mocker.GetMock<ITaskRepository>()
                .Verify(o => o.CreateAsync(task.Object));
        }

        [Test]
        public async Task CreateAsync_GroupIdAndValidData_CreatesTask()
        {
            //Arrange
            var sut = CreateSut();
            var dto = new CreateTaskDto("name", Guid.NewGuid(),DateTime.Now.AddDays(1));
            
            Tools.SetupCurrentUserService(_mocker);

            var taskId = Guid.NewGuid();

            var task = new Mock<ITaskEntity>();
            task.SetupGet(o => o.Name).Returns(dto.Name);
            task.SetupGet(o => o.Id).Returns(taskId);
            task.SetupGet(o => o.GroupId).Returns(dto.GroupId);
            task.SetupGet(o => o.Deadline).Returns(dto.Deadline);

            _mocker.GetMock<IEntityValidator>()
                .Setup(o => o.IsGroupIdValidAsync(dto.GroupId.Value))
                .ReturnsAsync(true);

            _mocker.GetMock<ITaskAggregate>()
                .Setup(o => o.Create(dto.Name, Tools.User.Id, dto.GroupId, dto.Deadline))
                .Returns(task.Object);
                            
            //Act
            var result = await sut.CreateAsync(dto);
            
            //Assert
            result.GroupId.ShouldBe(dto.GroupId);
            result.Id.ShouldBe(task.Object.Id);
            result.Name.ShouldBe(dto.Name);
            result.IsComplete.ShouldBeFalse();
            result.Deadline.ShouldBe(dto.Deadline);

        }

        [Test]
        public async Task GetUnGroupedTasksAsync_SingleResult_ReturnsWithCorrectValues()
        {
            //Arrange
            var sut = CreateSut();
            var task = new Mock<ITaskEntity>();
            var id = Guid.NewGuid();
            var created = DateTime.Now.AddHours(-2);
            var deadline = DateTime.Now.AddDays(1);
            
            Tools.SetupCurrentUserService(_mocker);
            
            task.SetupGet(o => o.Id).Returns(id);
            task.SetupGet(o => o.Name).Returns("name");
            task.SetupGet(o => o.Created).Returns(created);
            task.SetupGet(o => o.Deadline).Returns(deadline);
            task.SetupGet(o => o.IsComplete).Returns(false);
            task.SetupGet(o => o.UserId).Returns(Tools.User.Id);

            _mocker.GetMock<ITaskRepository>()
                .Setup(o => o.GetUngroupedTasksForUserAsync(Tools.User.Id, false))
                .ReturnsAsync(new []{task.Object});
            
            //Act
            var results = await sut.GetUngroupedTasksAsync();

            //Assert
            results.Count().ShouldBe(1);
            var taskDto = results.First();
            taskDto.Id.ShouldBe(task.Object.Id);
            taskDto.Name.ShouldBe(task.Object.Name);
            taskDto.Created.ShouldBe(task.Object.Created);
            taskDto.Deadline.ShouldBe(task.Object.Deadline);
            taskDto.GroupId.ShouldBeNull();
            taskDto.IsComplete.ShouldBe(task.Object.IsComplete);


        }
        
        

        private ITaskService CreateSut() => _mocker.CreateInstance<TaskService>();
    }
}