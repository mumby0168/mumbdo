using System;
using System.Runtime;
using Moq.AutoMock;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Domain.Tests.Aggregates
{
    public class ItemGroupAggregateTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void Create_EmptyName_AlwaysThrows()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            //Assert 
            var ex = Assert.Throws<FieldRequiredException>(() => sut.Create(Guid.NewGuid(), "", "", ""));
            ex.Message.ShouldContain("Name");
        }
        
        [Test]
        public void Create_EmptyUrl_AlwaysThrows()
        {
            //Arrange
            var sut = CreateSut();

            //Act
            //Assert 
            var ex = Assert.Throws<FieldRequiredException>(() => sut.Create(Guid.NewGuid(), "Test", "", ""));
            ex.Message.ShouldContain("URL");
        }

        [Test]
        public void Create_ValidData_ReturnsGroup()
        {
            //Arrange
            var sut = CreateSut();
            var name = "test";
            var desc = "description";
            var url = "http://test.com";
            var userId = Guid.NewGuid();


            //Act
            var group = sut.Create(userId, name, desc, url);

            //Assert
            group.Description.ShouldBe(desc);
            group.Name.ShouldBe(name);
            group.UserId.ShouldBe(userId);
            group.ImageUri.ShouldBe(url);
        }

        private IItemGroupAggregate CreateSut() => _mocker.CreateInstance<ItemGroupAggregate>();
    }
}