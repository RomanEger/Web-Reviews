using Contracts;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Repositories
{
    public class TestVideoRatingRepository
    {
        private IRepositoryManager repositoryManager;
        private Mock<WebReviewsContext> mockContext;
        private VideoRatingsFixture fixture;

        public TestVideoRatingRepository()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            fixture = new VideoRatingsFixture();
        }

        [Fact]
        public async void Get_OnSuccess_ReturnedListOf_VideoRatings_Count_2()
        {
            var expectedCount = 2;
            var videoRatings = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb");
            mockContext.Setup(x => x.Set<Videorating>()).Returns(videoRatings.Object);

            var result = await repositoryManager.VideoRating.GetVideoRatingsAsync(videoId, trackChanges: false);
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async void Get_OnSuccess_ReturnedVideoRating_WithUser()
        {
            var videoRatings = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb");
            var userId = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403");
            mockContext.Setup(x => x.Set<Videorating>()).Returns(videoRatings.Object);

            var result = await repositoryManager.VideoRating.GetVideoRatingAsync(videoId, userId, trackChanges: false);
            result.Should().NotBeNull();
        }

        [Fact]
        public async void Get_OnSuccess_CreatedVideoRating()
        {
            var created = false;
            var videoRating = fixture.GetTestData().First();
            mockContext.Setup(x => x.Set<Videorating>().Add(It.IsAny<Videorating>())).Callback(() => created = true);

            repositoryManager.VideoRating.CreateVideoRating(videoRating);
            created.Should().BeTrue();
        }

        [Fact]
        public async void Get_OnSuccess_DeletedVideoRating()
        {
            var deleted = false;
            var videoRating = fixture.GetTestData().First();
            mockContext.Setup(x => x.Set<Videorating>().Remove(It.IsAny<Videorating>())).Callback(() => deleted = true);

            repositoryManager.VideoRating.DeleteVideoRating(videoRating);
            deleted.Should().BeTrue();
        }
    }
}
