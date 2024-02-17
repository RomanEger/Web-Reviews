using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Service.Contracts;
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
        private AuthenticationController authenticationController;
        private IMapper mapper;
        private UserFixture fixture;

        public TestAuthenticationController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            authenticationController = new AuthenticationController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new UserFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_StatusCode201()
        {
            var user = fixture.GetTestData().First();

            var userForRegistrationDTO = mapper.Map<UserForRegistrationDTO>(user);

            mockServiceManager.Setup(x => x.Authentication.CreateUserAsync(It.IsAny<UserForRegistrationDTO>()));

            authenticationController.ModelState.TryAddModelError("APi", "Error");
            var result = await authenticationController.RegisterUser(userForRegistrationDTO);
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

            var result = await authenticationController.Authenticate(userForAuthentication);
            var status = result as StatusCodeResult;
            status.StatusCode.Should().Be(401);
        }
    }
}
