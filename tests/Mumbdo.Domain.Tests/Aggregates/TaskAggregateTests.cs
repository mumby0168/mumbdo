using System;
using Moq.AutoMock;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Domain.Tests.Aggregates
{
    public class TaskAggregateTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void Create_Always_CreatesTask()
        {
            //Arrange
            var sut = CreateSut();
            var name = "test-task";
            var userId = Guid.NewGuid();
            var groupId = Guid.NewGuid();
            var deadline = DateTime.Now.AddDays(1);

            //Act
            var task = sut.Create(name, userId, groupId, deadline);

            //Assert
            task.Id.ShouldNotBe(Guid.Empty);
            task.Name.ShouldBe(name);
            task.Created.ShouldBeLessThan(DateTime.Now);
            task.Deadline.ShouldBe(deadline);
            task.GroupId.ShouldBe(groupId);
            task.UserId.ShouldBe(userId);
            task.IsComplete.ShouldBeFalse();
        }

        [Test]
        public void Create_EmptyName_Throws()
        {
            //Arrange
            var sut = CreateSut();
            var name = "";
            
            //Act
            //Assert
            Assert.Throws<FieldRequiredException>(() => sut.Create(name, Guid.NewGuid(), null, null));
        }

        private ITaskAggregate CreateSut() => _mocker.CreateInstance<TaskAggregate>();
    }
}