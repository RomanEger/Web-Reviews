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
using WebReviews.API.Mapper;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Shared.DataTransferObjects;
using System.Linq.Expressions;

namespace WebReviews.Tests.Systems.Services
{
    public class TestVideoService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private Mock<IRepositoryManager> mockRepositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private VideoFixture fixture;

        public TestVideoService()
        {
            mockContext = new Mock<WebReviewsContext>();

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            autoMapper = mockAutoMapper.CreateMapper();
            mockRepositoryManager = new Mock<IRepositoryManager>();
            entityChecker = new EntityChecker(mockRepositoryManager.Object);
            options = Options.Create(new JwtConfiguration());
            serviceManager = new ServiceManager(mockRepositoryManager.Object, autoMapper, entityChecker, options);
            fixture = new VideoFixture();

            var videoType = new List<Videotype> { new() { Title = "some", VideoTypeId = Guid.NewGuid() } }.BuildMock().BuildMockDbSet();
            var videoStatus = new List<Videostatus> { new() { Title = "some", VideoStatusId = Guid.NewGuid() } }.BuildMock().BuildMockDbSet();
            var videoRestriction = new List<Videorestriction> { new() { Title = "some", VideoRestrictionId = Guid.NewGuid(), Description = "some" } }
                .BuildMock()
                .BuildMockDbSet();

            mockRepositoryManager.Setup(x => x.VideoType.GetGyConditionAsync(
                It.IsAny<Expression<Func<Videotype, bool>>>(),false))
                .Returns(Task.FromResult(videoType.Object.First()));

            mockRepositoryManager.Setup(x => x.VideoStatuses.GetGyConditionAsync(
                It.IsAny<Expression<Func<Videostatus, bool>>>(), false))
                .Returns(Task.FromResult(videoStatus.Object.First()));

            mockRepositoryManager.Setup(x => x.VideoRestriction.GetGyConditionAsync(
                It.IsAny<Expression<Func<Videorestriction, bool>>>(), false))
                .Returns(Task.FromResult(videoRestriction.Object.First()));
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedVideo()
        {
            var created = false;
            var videoEntity = fixture.GetTestData().First();

            var videoForManipulation = autoMapper.Map<VideoForManipulationDTO>(videoEntity);

            mockRepositoryManager.Setup(x => x.Video.CreateVideo(It.IsAny<Video>())).Callback(() =>
            {
                created = true;
            });
            mockRepositoryManager.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            await serviceManager.Video.CreateVideoAsync(videoForManipulation);

            created.Should().BeTrue();
            mockRepositoryManager.Verify(x => x.Video.CreateVideo(It.IsAny<Video>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_DeletedVideo()
        {
            var video = fixture.GetTestData().First();
            var deleted = false;
            mockRepositoryManager.Setup(x => x.Video.GetVideoAsync(
                It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(video));

            mockRepositoryManager.Setup(x => x.Video.DeleteVideo(It.IsAny<Video>())).Callback(() =>
            {
                deleted = true;
            });
            mockRepositoryManager.Setup(x => x.SaveAsync()).Returns(Task.CompletedTask);

            await serviceManager.Video.DeleteVideoAsync(Guid.NewGuid(), true);

            deleted.Should().BeTrue();
            mockRepositoryManager.Verify(x => x.Video.DeleteVideo(It.IsAny<Video>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_VideoById()
        {
            var video = fixture.GetTestData().First();
            mockRepositoryManager.Setup(x => x.Video.GetVideoAsync(
                It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(video));

            var result = await serviceManager.Video.GetVideoByIdAsync(Guid.NewGuid(), true);

            mockRepositoryManager.Verify(x => x.Video.GetVideoAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once());
            result.Title.Should().BeEquivalentTo(video.Title);
        }

        [Fact]
        public async Task Get_OnSuccess_UpdatedVideo()
        {
            var video = fixture.GetTestData().First();
            mockRepositoryManager.Setup(x => x.Video.GetVideoAsync(
                It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(video));

            var videoForManipulation = new VideoForManipulationDTO
            {
                Title = "None",
                Description = video.Description,
                Photo = video.Photo,
                VideoRestrictionId = video.VideoRestrictionId,
                VideoStatusId = video.VideoStatusId,
                VideoTypeId = video.VideoTypeId,
                CurrentEpisode = video.CurrentEpisode,
                ReleaseDate = video.ReleaseDate,
                Rating = video.Rating,
                TotalEpisodes = video.TotalEpisodes
            };

            var result = await serviceManager.Video.UpdateVideoAsync(Guid.NewGuid(),videoForManipulation, true);
            result.Title.Should().BeEquivalentTo(videoForManipulation.Title);
        }
    }
}
