using Contracts;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Repositories
{
    public class TestVideoRepository
    {
        private IRepositoryManager repositoryManager;
        private Mock<WebReviewsContext> mockContext;
        private VideoFixture fixture;

        public TestVideoRepository()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            fixture = new VideoFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_Count_5()
        {
            var allDbEntitiesCount = 15;
            var expectedCount = 5;
            var listOfVideo = fixture.GetRandomData(allDbEntitiesCount).BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters { PageNumber = 2};

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.Should().HaveCount(expectedCount);
            result.MetaData.HasPrevious.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_Count_1_WithName_Demon()
        {
            var expectedCount = 1;
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                SearchTitle = "Demon"
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.Should().HaveCount(expectedCount);
            result.MetaData.HasPrevious.Should().BeFalse();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_OrderByDescending_Date()
        {
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                DateFiltering = true
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.First().Title.Should().Be("Demon Slayer");
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_OrderByDescending_Rating()
        {
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                RatingFiltering = true
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.First().Title.Should().Be("Bakuman");
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_OrderByAlphabet()
        {
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                AlphabetFiltering = true
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.First().Title.Should().Be("Bakuman");
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_OrderByAlphabet_And_Date()
        {
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                AlphabetFiltering = true,
                DateFiltering = true
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.First().Title.Should().Be("Bakuman");
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedListOfVideo_OrderByAlphabet_And_Title()
        {
            var expectedCount = 2;
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoParameters = new VideoParameters
            {
                AlphabetFiltering = true,
                SearchTitle = "d"
            };

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Video>>();
            result.First().Title.Should().Be("Demon Slayer");
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedVideo_WithId()
        {
            var videoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb");
            var listOfVideo = fixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Video>()).Returns(listOfVideo.Object);

            var result = await repositoryManager.Video.GetVideoAsync(videoId, trackChanges: false);

            result.Should().NotBeNull();            
        }

        [Fact]
        public void Get_OnSuccess_CreatedVideo()
        {
            var created = false;
            var video = fixture.GetTestData().First();

            mockContext.Setup(x => x.Set<Video>().Add(It.IsAny<Video>())).Callback(() =>
            {
                created = true;
            });

            repositoryManager.Video.CreateVideo(video);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<Video>().Add(It.IsAny<Video>()), Times.Once);
        }

        [Fact]
        public void Get_OnSuccess_DeletedVideo()
        {
            var deleted = false;
            var video = fixture.GetTestData().First();

            mockContext.Setup(x => x.Set<Video>().Remove(It.IsAny<Video>())).Callback(() =>
            {
                deleted = true;
            });

            repositoryManager.Video.DeleteVideo(video);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<Video>().Remove(It.IsAny<Video>()), Times.Once);
        }
    }
}
