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
using Shared.RequestFeatures;

namespace WebReviews.Tests.Systems.Services
{
    public class TestVideoService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private Mock<IRepositoryManager> mockRepositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private IServiceManager serviceManagerMockRepos;
        private VideoFixture fixture;

        public TestVideoService()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            autoMapper = mockAutoMapper.CreateMapper();
            mockRepositoryManager = new Mock<IRepositoryManager>();
            entityChecker = new EntityChecker(mockRepositoryManager.Object);
            options = Options.Create(new JwtConfiguration());
            serviceManagerMockRepos = new ServiceManager(mockRepositoryManager.Object, autoMapper, entityChecker, options);
            serviceManager = new ServiceManager(repositoryManager, autoMapper, entityChecker, options);
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

            await serviceManagerMockRepos.Video.CreateVideoAsync(videoForManipulation);

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

            await serviceManagerMockRepos.Video.DeleteVideoAsync(Guid.NewGuid(), true);

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

            var result = await serviceManagerMockRepos.Video.GetVideoByIdAsync(Guid.NewGuid(), true);

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

            var result = await serviceManagerMockRepos.Video.UpdateVideoAsync(Guid.NewGuid(),videoForManipulation, true);
            result.Title.Should().BeEquivalentTo(videoForManipulation.Title);
        }

        [Fact]
        public async Task Get_OnSuccesVideos_With_2_NecessaryGenres_Count_1()
        {
            var videos = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var expectedCount = 1;
            var videoGenres = new List<Videogenre>
            {
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("f63682c9-95be-4b3c-a87d-33fc08083391")},
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},
                new() { VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},

            }.BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                GenreIds = new List<Guid>
                {   
                    new Guid("f63682c9-95be-4b3c-a87d-33fc08083391"),
                    new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")
                }
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(videos.Object);
            mockContext.Setup(x => x.Set<Videogenre>()).Returns(videoGenres.Object);

            var result = await serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: true);

            result.videos.Should().HaveCount(expectedCount);
            result.metaData.TotalCount.Should().Be(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccesVideos_With_1_NecessaryGenre_Count_2()
        {
            var videos = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var expectedCount = 2;
            var videoGenres = new List<Videogenre>
            {
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("f63682c9-95be-4b3c-a87d-33fc08083391")},
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},
                new() { VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},

            }.BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                GenreIds = new List<Guid>
                {   
                    new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")
                }
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(videos.Object);
            mockContext.Setup(x => x.Set<Videogenre>()).Returns(videoGenres.Object);

            var result = await serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: true);

            result.videos.Should().HaveCount(expectedCount);
            result.metaData.TotalCount.Should().Be(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccesVideos_With_1_NecessaryGenre_AndFilterRating_Count_2()
        {
            var videos = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var expectedCount = 2;
            var videoGenres = new List<Videogenre>
            {
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("f63682c9-95be-4b3c-a87d-33fc08083391")},
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},
                new() { VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},

            }.BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                RatingFiltering = true,
                GenreIds = new List<Guid>
                {
                    new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")
                }
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(videos.Object);
            mockContext.Setup(x => x.Set<Videogenre>()).Returns(videoGenres.Object);

            var result = await serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: true);

            result.videos.Should().HaveCount(expectedCount);
            result.metaData.TotalCount.Should().Be(expectedCount);
            result.videos.First().Title.Should().BeEquivalentTo("Demon Slayer");
        }

        [Fact]
        public async Task Get_OnSuccesVideos_With_1_NecessaryGenre_AndFilterSearchTitle_Count_1()
        {
            var videos = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var expectedCount = 1;
            var videoGenres = new List<Videogenre>
            {
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("f63682c9-95be-4b3c-a87d-33fc08083391")},
                new() { VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},
                new() { VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb") , GenreId = new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")},

            }.BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                SearchTitle = "Dunter",
                GenreIds = new List<Guid>
                {
                    new Guid("8e12c33c-97ec-44b4-ad20-c5cd2e24786d")
                }
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(videos.Object);
            mockContext.Setup(x => x.Set<Videogenre>()).Returns(videoGenres.Object);

            var result = await serviceManager.Video.GetVideosAsync(videoParameters, trackChanges: true);

            result.videos.Should().HaveCount(expectedCount);
            result.metaData.TotalCount.Should().Be(expectedCount);
            result.videos.First().Title.Should().BeEquivalentTo("Dunter x Hunter");
        }
    }
}
