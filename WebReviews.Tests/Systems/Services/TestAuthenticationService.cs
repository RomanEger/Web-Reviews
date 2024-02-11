using AutoMapper;
using Entities.Models;
using FluentAssertions;
using Moq;
using Repository;
using Service.Helpers;
using Service;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;
using WebReviews.API.Mapper;
using MockQueryable.Moq;
using Microsoft.Extensions.Options;
using Entities.ConfigurationModels;

namespace WebReviews.Tests.Systems.Services
{
    public class TestAuthenticationService
    {
        [Fact]
        public async Task Get_OnSuccess_Created_User_And_Returned_User()
        {
            var fixture = new UserFixture();

            var created = false;
            var users = fixture.GetRandomData(2).BuildMock().BuildMockDbSet();
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();

            var userForCreation = new UserForRegistrationDTO
            {
                Nickname = "Oleg",
                Email = "oleg@mail.ru",
                Password = "1234",
                UserRankId = userRankGuid
            };

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<User>().Add(It.IsAny<User>())).Callback(() =>
            {
                created = true;
            });
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);
            var entityChecker = new EntityChecker(repositoryManager);
            var options = Options.Create(new JwtConfiguration());

            var serviceManager = new ServiceManager(repositoryManager, autoMapper, entityChecker, options);

            await serviceManager.Authentication.CreateUserAsync(userForCreation);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Add(It.IsAny<User>()), Times.Once());
        }
    }
}
