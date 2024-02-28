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
    }
}
