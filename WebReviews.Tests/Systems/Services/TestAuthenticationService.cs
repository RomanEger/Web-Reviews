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
using Contracts;
using Service.Contracts;

namespace WebReviews.Tests.Systems.Services
{
    public class TestAuthenticationService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private UserFixture fixture;
        public TestAuthenticationService()
        {
            mockContext = new Mock<WebReviewsContext>();

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            autoMapper = mockAutoMapper.CreateMapper();
            repositoryManager = new RepositoryManager(mockContext.Object);
            entityChecker = new EntityChecker(repositoryManager);
            options = Options.Create(new JwtConfiguration());
            serviceManager = new ServiceManager(repositoryManager, autoMapper, entityChecker, options);
            fixture = new UserFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_Created_User()
        {
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

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<User>().Add(It.IsAny<User>())).Callback(() =>
            {
                created = true;
            });
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);


            await serviceManager.Authentication.CreateUserAsync(userForCreation);
                        
            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Created_Access_And_RefreshTokens()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();

            var userForCreation = new UserForAuthenticationDTO
            {
                Nickname = "MakkLaud",
                Password = "password"
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);

            var options = Options.Create(new JwtConfiguration
            {
                ValidIssuer = "IRateAPI",
                ValidAudience = "IRateHttps",
                Expires = "5",
                RefreshTokenExpiresDays = "3",
                SecretKey = "Secret key which we need to change, mb put in environment"
            });

            var serviceManager = new ServiceManager(repositoryManager, autoMapper, entityChecker, options);
            await serviceManager.Authentication.ValidateUser(userForCreation);

            var tokens = await serviceManager.Authentication.CreateToken(populateExp: true);

            tokens.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Refreshed_AccessToken()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();

            var userForCreation = new UserForAuthenticationDTO
            {
                Nickname = "MakkLaud",
                Password = "password"
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);


            var options = Options.Create(new JwtConfiguration
            {
                ValidIssuer = "IRateAPI",
                ValidAudience = "IRateHttps",
                Expires = "5",
                RefreshTokenExpiresDays = "3",
                SecretKey = "Secret key which we need to change, mb put in environment"
            });

            var serviceManager = new ServiceManager(repositoryManager, autoMapper, entityChecker, options);
            await serviceManager.Authentication.ValidateUser(userForCreation);

            var tokens = await serviceManager.Authentication.CreateToken(populateExp: true);

            var newTokens = await serviceManager.Authentication.RefreshToken(tokens);

            tokens.RefreshToken.Should().NotBeEquivalentTo(newTokens.RefreshToken);
        }
    }
}
