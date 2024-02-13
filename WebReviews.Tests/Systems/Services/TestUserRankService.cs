using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Microsoft.Extensions.Options;
using Moq;
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
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using WebReviews.API.Mapper;
using Entities.Exceptions;

namespace WebReviews.Tests.Systems.Services
{
    public class TestUserRankService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;

        public TestUserRankService()
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
        }


        [Fact]
        public async Task Get_OnSuccess_Returned_UserRank_With_Id()
        {
            var userRankGuid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");

            var userRankEntities = new List<Userrank>
            {
                new Userrank {
                    UserRankId = userRankGuid,
                    Title = "God",
                    Description = "Some"
                }
            }.BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRankEntities.Object);


            var entity = await serviceManager.UserRank.GetUserRankAsync(userRankGuid, trackChanges: true);

            entity.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_NotFoundException()
        {
            var userRankGuid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");

            var userRankEntities = new List<Userrank>
            {
                new Userrank {
                    UserRankId = userRankGuid,
                    Title = "God",
                    Description = "Some"
                }
            }.BuildMock().BuildMockDbSet();

            var newGuid = Guid.NewGuid();

            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRankEntities.Object);

            await serviceManager.Invoking(async x => await x.UserRank.GetUserRankAsync(newGuid, trackChanges: true))
                .Should()
                .ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_ListOf_UserRanks_Count_1()
        {
            var expectedCount = 1;
            var userRankEntities = new List<Userrank>
            {
                new Userrank {
                    UserRankId = Guid.NewGuid(),
                    Title = "God",
                    Description = "Some"
                }
            }.BuildMock().BuildMockDbSet();


            mockContext.Setup(x => x.Set<Userrank>()).Returns(userRankEntities.Object);

            var result = await serviceManager.UserRank.GetUserRanksAsync(trackChanges: true);

            result.Should().HaveCount(expectedCount);
        }
    }
}
