using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Microsoft.Extensions.Options;
using Moq;
using Presentation.Controllers;
using Repository;
using Service.Contracts;
using Service.Helpers;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;
using WebReviews.API.Mapper;
using Entities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
using Shared.DataTransferObjects;

namespace WebReviews.Tests.Systems.Controllers
{
    public class TestTokenController
    {
        private TokenController tokenController;
        private Mock<WebReviewsContext> mockContext;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IMapper mapper;
        private UserFixture fixture;
        private IServiceManager serviceManager;

        public TestTokenController()
        {
            mockContext = new Mock<WebReviewsContext>();

            repositoryManager = new RepositoryManager(mockContext.Object);
            entityChecker = new EntityChecker(repositoryManager);
            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            options = Options.Create(new JwtConfiguration
            {
                ValidIssuer = "IRateAPI",
                ValidAudience = "IRateHttps",
                Expires = "30",
                RefreshTokenExpiresDays = "3",
                SecretKey = "Secret key which we need to change, mb put in environment"
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new UserFixture();
            serviceManager = new ServiceManager(repositoryManager, mapper, entityChecker, options);
            tokenController = new TokenController(serviceManager);
        }

        [Fact]
        public async Task Get_OnSuccess_UserByAccessToken()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();

            var userForAuth = new UserForAuthenticationDTO
            {
                UserPersonalData = "MakkLaud",
                Password = "password"
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);

            var options = Options.Create(new JwtConfiguration
            {
                ValidIssuer = "IRateAPI",
                ValidAudience = "IRateHttps",
                Expires = "30",
                RefreshTokenExpiresDays = "3",
                SecretKey = "Secret key which we need to change, mb put in environment"
            });

            var serviceManager = new ServiceManager(repositoryManager, mapper, entityChecker, options);
            await serviceManager.Authentication.ValidateUser(userForAuth);

            var tokens = await serviceManager.Authentication.CreateToken(populateExp: true);

            tokens.Should().NotBeNull();


            var result = await tokenController.GetUserByAccessToken(tokens.AccessToken);
            var okResult = result as OkObjectResult;
            var user = okResult.Value as UserDTO;
            user.Nickname.Should().BeEquivalentTo(userForAuth.UserPersonalData);
        }

        [Fact]
        public async Task Get_OnSuccess_RefreshedAccessToken()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();

            var userForAuth = new UserForAuthenticationDTO
            {
                UserPersonalData = "MakkLaud",
                Password = "password"
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);

            var options = Options.Create(new JwtConfiguration
            {
                ValidIssuer = "IRateAPI",
                ValidAudience = "IRateHttps",
                Expires = "30",
                RefreshTokenExpiresDays = "3",
                SecretKey = "Secret key which we need to change, mb put in environment"
            });

            var serviceManager = new ServiceManager(repositoryManager, mapper, entityChecker, options);
            await serviceManager.Authentication.ValidateUser(userForAuth);

            var tokens = await serviceManager.Authentication.CreateToken(populateExp: true);

            tokens.Should().NotBeNull();


            var result = await tokenController.RefreshToken(tokens);
            var okResult = result as OkObjectResult;
            var returnedTokens = okResult.Value as TokenDTO;
            returnedTokens.Should().NotBeNull();
        }
    }
}
