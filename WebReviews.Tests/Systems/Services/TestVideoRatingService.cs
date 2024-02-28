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
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;
using MockQueryable.Moq;
using Entities.Models;
using Shared.DataTransferObjects;
using FluentAssertions;

namespace WebReviews.Tests.Systems.Services
{
    public class TestVideoRatingService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private VideoRatingsFixture videoRatingFixture;
        private VideoFixture videoFixture;
        private UserFixture userFixture;

        public TestVideoRatingService()
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
            videoRatingFixture = new VideoRatingsFixture();
            userFixture = new UserFixture();
            videoFixture = new VideoFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedVideoRating()
        {
            var created = false;
            var videoRating = videoRatingFixture.GetTestData().First();
            var videoRatingsToReturn = videoRatingFixture.GetTestData().BuildMock().BuildMockDbSet();
            var usersToReturn = userFixture.GetTestData().BuildMock().BuildMockDbSet();
            var videosToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();

            videoRating.VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb");
            var videoRatingForManipulation = autoMapper.Map<VideoRatingForManipulationDTO>(videoRating);

            mockContext.Setup(x => x.Set<Videorating>()).Returns(videoRatingsToReturn.Object);
            mockContext.Setup(x => x.Set<User>()).Returns(usersToReturn.Object);
            mockContext.Setup(x => x.Set<Video>()).Returns(videosToReturn.Object);
            mockContext.Setup(x => x.Set<Videorating>().Add(It.IsAny<Videorating>())).Callback(() => created = true);

            var result = await serviceManager.VideoRating.CreateVideoRating(videoRatingForManipulation, trackChanges: true);

            created.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_UpdatedVideoRating()
        {
            var created = false;
            var videoRating = videoRatingFixture.GetTestData().First();
            var videoRatingsToReturn = videoRatingFixture.GetTestData().BuildMock().BuildMockDbSet();
            var usersToReturn = userFixture.GetTestData().BuildMock().BuildMockDbSet();
            var videosToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();

            videoRating.Rating = 1;
            var videoRatingForManipulation = autoMapper.Map<VideoRatingForManipulationDTO>(videoRating);

            mockContext.Setup(x => x.Set<Videorating>()).Returns(videoRatingsToReturn.Object);
            mockContext.Setup(x => x.Set<User>()).Returns(usersToReturn.Object);
            mockContext.Setup(x => x.Set<Video>()).Returns(videosToReturn.Object);
            mockContext.Setup(x => x.Set<Videorating>().Add(It.IsAny<Videorating>())).Callback(() => created = true);

            var result = await serviceManager.VideoRating.CreateVideoRating(videoRatingForManipulation, trackChanges: true);

            created.Should().BeFalse();
        }

        [Fact]
        public async Task Get_OnSuccess_RefreshedVideo_Rating()
        {
            var videoRating = videoRatingFixture.GetTestData().First();
            var videoRatingsToReturn = videoRatingFixture.GetTestData().BuildMock().BuildMockDbSet();
            var videosToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videorating>()).Returns(videoRatingsToReturn.Object);
            mockContext.Setup(x => x.Set<Video>()).Returns(videosToReturn.Object);

            var result = await serviceManager.VideoRating.RefreshVideoRating(videoRating.VideoId, trackChanges: true);

            result.Should().NotBeNull();
        }
    }
}
