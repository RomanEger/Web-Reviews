using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Presentation.Controllers;
using Repository;
using Service;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Controllers
{
    public class TestAuthenticationController
    {
        private Mock<IServiceManager> mockServiceManager;
        private AuthenticationController autControllerWithMockService;
        private AuthenticationController authenticationController;
        private Mock<WebReviewsContext> mockContext;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IMapper mapper;
        private UserFixture fixture;
        private IServiceManager serviceManager;

        public TestAuthenticationController()
        {
            mockContext = new Mock<WebReviewsContext>();
            mockServiceManager = new Mock<IServiceManager>();
            autControllerWithMockService = new AuthenticationController(mockServiceManager.Object);
            
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
            authenticationController = new AuthenticationController(serviceManager);
        }

        [Fact]
        public async Task Get_OnSuccess_StatusCode201()
        {
            var user = fixture.GetTestData().First();

            var userForRegistrationDTO = mapper.Map<UserForRegistrationDTO>(user);

            mockServiceManager.Setup(x => x.Authentication.CreateUserAsync(It.IsAny<UserForRegistrationDTO>()));

            autControllerWithMockService.ModelState.TryAddModelError("APi", "Error");
            var result = await autControllerWithMockService.RegisterUser(userForRegistrationDTO);
            var status = result as StatusCodeResult;
            status.StatusCode.Should().Be(201);
        }

        [Fact]
        public async Task Get_OnSuccess_StatusCode401()
        {
            var user = fixture.GetTestData().First();

            var userForAuthentication = new UserForAuthenticationDTO 
            { 
                UserPersonalData = user.Nickname ,
                Password = user.Password
            };

            mockServiceManager.Setup(x => x.Authentication.ValidateUser(It.IsAny<UserForAuthenticationDTO>()))
                .Returns(Task.FromResult(false));

            var result = await autControllerWithMockService.Authenticate(userForAuthentication);
            var status = result as StatusCodeResult;
            status.StatusCode.Should().Be(401);
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


            var result = await authenticationController.GetUserByAccessToken(tokens);
            var okResult = result as OkObjectResult;
            var user = okResult.Value as UserDTO;
            user.Nickname.Should().BeEquivalentTo(userForAuth.UserPersonalData);
        }
    }
}
