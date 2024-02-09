﻿using AutoMapper;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using Service;
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
    public class TestUserService
    {
        [Fact]
        public async Task Get_OnSuccess_Created_User_And_Returned_User()
        {
            var fixture = new UserFixture();

            var created = false;
            var users = fixture.GetRandomData(2).BuildMock().BuildMockDbSet();

            var userForCreation = new UserForRegistrationDTO
            {
                Nickname = "Oleg",
                Email = "oleg@mail.ru",
                Password = "1234",
                UserRankId = new Guid()
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

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            await serviceManager.User.CreateUserAsync(userForCreation);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Deleted_User()
        {
            var fixture = new UserFixture();

            var deleted = false;
            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);
            mockContext.Setup(x => x.Set<User>().Remove(It.IsAny<User>())).Callback(() =>
            {
                deleted = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            await serviceManager.User.DeleteUserAsync(guid, trackChanges: true);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Remove(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_ListOfUsers_With_Count_3()
        {
            var fixture = new UserFixture();

            var expectedCount = 3;
            var users = fixture.GetRandomData(expectedCount).BuildMock().BuildMockDbSet();


            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);


            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var listOfUsers = await serviceManager.User.GetUsersAsync(trackChanges: false);

            listOfUsers.Should().HaveCount(expectedCount);  
        }

        [Fact]
        public async Task Get_OnSuccess_User_With_Id()
        {
            var fixture = new UserFixture();

            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);


            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var user = await serviceManager.User.GetUserByIdAsync(guid, trackChanges: false);

            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Updated_User_And_Returned_User()
        {
            var fixture = new UserFixture();

            var users = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403");

            var userForUpdate = new UserForUpdateDTO
            {
                Nickname = "makklaud",
                Password = "password",
                UserRankId = new Guid()                
            };

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(users.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var updateUser = await serviceManager.User.UpdateUserAsync(guid, userForUpdate, trackChanges: true);

            updateUser.Nickname.Should().Be("makklaud");
        }
    }
}