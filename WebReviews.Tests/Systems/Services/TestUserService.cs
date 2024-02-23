using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
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

namespace WebReviews.Tests.Systems.Services
{
    [TestFixture]
    public class TestUserService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private UserFixture fixture;

        public TestUserService()
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
        public async Task Get_OnSuccess_Deleted_User()
        {
            var deleted = false;
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<User>().Remove(It.IsAny<User>())).Callback(() =>
            {
                deleted = true;
            });

            await serviceManager.User.DeleteUserAsync(guid, trackChanges: true);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Remove(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_ListOfUsers_With_Count_3()
        {
            var expectedCount = 3;
            var users = fixture.GetRandomData(expectedCount).BuildMock().BuildMockDbSet();


            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);

            var listOfUsers = await serviceManager.User.GetUsersAsync(trackChanges: false);

            listOfUsers.Should().HaveCount(expectedCount);  
        }

        [Fact]
        public async Task Get_OnSuccess_User_With_Id()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);

            var user = await serviceManager.User.GetUserByIdAsync(guid, trackChanges: false);

            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Updated_User_And_Returned_User()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403");
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");
            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();
            var userForUpdate = new UserForUpdateDTO
            {
                Nickname = "makklaud228",
                Password = "password",
                UserRankId = userRankGuid             
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);


            var updateUser = await serviceManager.User.UpdateUserAsync(guid, userForUpdate, trackChanges: true);

            updateUser.Nickname.Should().Be("makklaud228");
        }

        [Fact]
        public async Task Get_OnFailed_Updated_User_And_Returned_NotFoundException()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403");
            var userRanks = new List<Userrank>() { new() { UserRankId = Guid.NewGuid(), Title = "Бог" } }.BuildMock().BuildMockDbSet();
            var userForUpdate = new UserForUpdateDTO
            {
                Nickname = "makklaud23",
                Password = "password",
                UserRankId = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403")
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);


            await serviceManager.Invoking(async x => await x.User.UpdateUserAsync(guid, userForUpdate, trackChanges: true))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Get_OnSuccess_Updated_User_With_Password()
        {
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");
            var userRankGuid = new Guid("3ab56b8e-c3ae-45c3-b9cb-f1a313a61ae5");

            var userRanks = new List<Userrank>() { new() { UserRankId = userRankGuid, Title = "Бог" } }.BuildMock().BuildMockDbSet();
            var userForUpdate = new UserForUpdateDTO
            {
                Nickname = "makklaud222",
                Password = "password1",
                UserRankId = userRankGuid
            };

            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRanks.Object);

            var updateUser = await serviceManager.User.UpdateUserAsync(guid, userForUpdate, trackChanges: true);

            updateUser.Nickname.Should().Be("makklaud222");
            updateUser.Password.Should().Be("cGFzc3dvcmQx");
        }
    }
}
