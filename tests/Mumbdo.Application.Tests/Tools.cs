using System;
using Moq.AutoMock;
using Mumbdo.Application.Jwt;
using Mumbdo.Shared;

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
    }
}