using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Controllers
{
    public class TestUserController
    {
        private Mock<IServiceManager> mockServiceManager;
        private UserController userController;
        private IMapper mapper;
        private UserFixture fixture;

        public TestUserController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            userController = new UserController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new UserFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ListOfUser_Length_5()
        {
            var expectedCount = 5;
            var listOfUsers = fixture.GetRandomData(expectedCount);

            var listOfUsersDTO = mapper.Map<IEnumerable<UserDTO>>(listOfUsers);

            mockServiceManager.Setup(x => x.User.GetUsersAsync(false)).Returns(Task.FromResult(listOfUsersDTO));

            var resultUser = await userController.GetUsers();

            var okResult = resultUser as OkObjectResult;
            var returnedListUsers = okResult.Value as IEnumerable<UserDTO>;

            returnedListUsers.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_User_With_Id()
        {
            var listOfUsers = fixture.GetTestData().First();
            var userId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            var userDTO = mapper.Map<UserDTO>(listOfUsers);

            mockServiceManager.Setup(x => x.User.GetUserByIdAsync(userId, false)).Returns(Task.FromResult(userDTO));

            var resultUser = await userController.GetUser(userId);

            var okResult = resultUser as OkObjectResult;
            var returnedUser = okResult.Value as UserDTO;

            returnedUser.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task Get_OnSuccess_Delete_User_WithId()
        {
            var listOfUsers = fixture.GetTestData();
            var userId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");
            var deleted = false;

            var listOfUsersDTO = mapper.Map<IEnumerable<UserDTO>>(listOfUsers);

            mockServiceManager.Setup(x => x.User.DeleteUserAsync(userId, false)).Callback(() =>
            {
                deleted = true;
            });

            await userController.DeleteUser(userId);

            deleted.Should().BeTrue();
            mockServiceManager.Verify(x => x.User.DeleteUserAsync(It.IsAny<Guid>(), false), Times.Once);
        }

        [Fact]
        public async Task Get_OnSuccess_Updated_User_WithId()
        {
            var user = fixture.GetTestData().First();
            var userId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            var userForUpdates = mapper.Map<UserForUpdateDTO>(user);
            var userDTO = mapper.Map<UserDTO>(user);

            mockServiceManager.Setup(x => x.User.UpdateUserAsync(userId, userForUpdates, false)).Returns(Task.FromResult(userDTO));

            var resutl = await userController.UpdateUser(userForUpdates, userId);

            mockServiceManager.Verify(x => x.User.UpdateUserAsync(userId, It.IsAny<UserForUpdateDTO>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
