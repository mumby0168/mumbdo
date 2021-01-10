using System;
using Moq;
using Moq.AutoMock;
using Mumbdo.Application.Jwt;
using Mumbdo.Application.Transport;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Tests
{
    public static class Tools
    {
        private static Guid _userId = Guid.NewGuid();
        public static CurrentUser User => new CurrentUser(_userId, "test@test.com", "user");

        public static void SetupCurrentUserService(AutoMocker mocker)
        {
            mocker.GetMock<ICurrentUserService>()
                .Setup(o => o.GetCurrentUser())
                .Returns(User);
        }
        
        public static Mock<ITransferDataService> CreateMockTransferService() => new();

        public static Mock<ITransferDataService> SetupTransferServiceExtensions()
        {
            var mock = CreateMockTransferService();
            DtoExtensions.SetMockedImplementation(mock.Object);
            return mock;
        }

        public static TaskDto DefaultTaskDto => new TaskDto(Guid.NewGuid(), "name", DateTime.Now, true, null, null);
    }
}