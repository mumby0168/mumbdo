using System;
using NUnit.Framework;
using Shouldly;

namespace Mumbdo.Shared.Tests.Models
{
    public class SignedInUserTests
    {
        [Test]
        public void Ctor_ValidToken_AssignsValues()
        {
            //Arrange
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJiNGRmYmZlOC04ZWFiLTQyZjktYjNiZS04MTBkYTJlNGYyM2EiLCJlbWFpbCI6Indqbm0ubXVtYnkxQGdtYWlsLmNvbSIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNjA4Njc1OTM3LCJleHAiOjE2MDg2ODY3MzcsImlhdCI6MTYwODY3NTkzN30.c32OGa9ZoCnijWHnQlLf-akJo12BRvaONBptIY9Y_mIbQVoI3OmMayuA46-odB25hhS18gyXjwmCfmC9iVHktQ";

            //Act
            var user = new SignedInUser(token);

            //Assert
            user.Email.ShouldBe("wjnm.mumby1@gmail.com");
            user.Id.ShouldBe(Guid.Parse("b4dfbfe8-8eab-42f9-b3be-810da2e4f23a"));
            user.Role.ShouldBe("user");
        }
        
    }
}