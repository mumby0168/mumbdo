using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Interfaces.Utilities;
using Mumbdo.Application.Services;
using Mumbdo.Application.Transport;
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
            DtoExtensions.Reset();
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

        [Test]
        public async Task UpdateAsync_ValidTaskToUpdate_Updates()
        {
            //Arrange
            Tools.SetupCurrentUserService(_mocker);
            var sut = CreateSut();
            var update = new UpdateTaskDto(Guid.NewGuid(), "update", true, Guid.NewGuid(), null);
            var transferServiceExtensions = Tools.SetupTransferServiceExtensions();

            var task = new Mock<ITaskEntity>();
            
            

            transferServiceExtensions.Setup(o => o.AsTaskDto(task.Object))
                .Returns(Tools.DefaultTaskDto);

            _mocker.GetMock<ITaskRepository>()
                .Setup(o => o.GetAsync(update.Id, Tools.User.Id))
                .ReturnsAsync(task.Object);

            //Act
            var result = await sut.UpdateAsync(update);

            //Assert
            result.ShouldNotBeNull();
            transferServiceExtensions.Verify(o => o.AsTaskDto(task.Object));
            task.Verify(o => o.Update(update.Name, update.IsComplete, update.GroupId, update.Deadline));
            _mocker.GetMock<ITaskRepository>()
                .Verify(o => o.UpdateAsync(task.Object));
        }

        [Test]
        public void UpdateAsync_BadId_Throws()
        {
            //Arrange
            var sut = CreateSut();
            Tools.SetupCurrentUserService(_mocker);

            //Act
            //Assert
            Assert.ThrowsAsync<TaskIdInvalidException>(() =>
                sut.UpdateAsync(new UpdateTaskDto(Guid.NewGuid(), "", true, Guid.NewGuid(), null)));
        }
        
        [Test]
        public void DeleteAsync_BadId_Throws()
        {
            //Arrange
            var sut = CreateSut();
            Tools.SetupCurrentUserService(_mocker);

            //Act
            //Assert
            Assert.ThrowsAsync<TaskIdInvalidException>(() =>
                sut.DeleteAsync(Guid.NewGuid()));
        }
        
        [Test]
        public async Task DeleteAsync_ValidId_DeletesTask()
        {
            //Arrange
            var sut = CreateSut();
            var id = Guid.NewGuid();
            Tools.SetupCurrentUserService(_mocker);
            var task = new Mock<ITaskEntity>();
            _mocker.GetMock<ITaskRepository>()
                .Setup(o => o.GetAsync(id, Tools.User.Id))
                .ReturnsAsync(task.Object);

            //Act
            await sut.DeleteAsync(id);

            //Assert
            _mocker.GetMock<ITaskRepository>()
                .Verify(o => o.DeleteAsync(id));
        }

        private ITaskService CreateSut() => _mocker.CreateInstance<TaskService>();
    }
}