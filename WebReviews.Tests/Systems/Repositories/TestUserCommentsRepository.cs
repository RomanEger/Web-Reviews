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
    public class TestUserCommentsRepository
    {
        private IRepositoryManager repositoryManager;
        private Mock<WebReviewsContext> mockContext;
        private CommentsFixture fixture;

        public TestUserCommentsRepository()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            fixture = new CommentsFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedPageList_Count_2()
        {
            var expectedCount = 2;
            var userComments = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var userCommentsParameters = new UserCommentsParameters();

            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userComments.Object);

            var result = await repositoryManager.UserComments.GetUserCommentsAsync(videoId, userCommentsParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Usercomment>>();
            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedPageList_Count_2_WithDateFiltering()
        {
            var expectedCount = 2;
            var userComments = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var userCommentsParameters = new UserCommentsParameters() { DateFiltering = true};

            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userComments.Object);

            var result = await repositoryManager.UserComments.GetUserCommentsAsync(videoId, userCommentsParameters, trackChanges: false);

            result.Should().BeOfType<PagedList<Usercomment>>();
            result.Should().HaveCount(expectedCount);
            result.First().Text.Should().BeEquivalentTo("Call");
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComment_WithId()
        {
            var userComments = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var commentId = new Guid("31bad2cd-024a-4448-a5a8-4df61e37be10");

            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userComments.Object);

            var result = await repositoryManager.UserComments.GetUserCommentByIdAsync(videoId, commentId, trackChanges: false);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedUserComment()
        {
            var created = false;
            var userComments = fixture.GetRandomData(1);

            mockContext.Setup(x => x.Set<Usercomment>().Add(It.IsAny<Usercomment>())).Callback(() =>
            {
                created = true;
            });

            repositoryManager.UserComments.CreateUserComment(userComments.First());

            mockContext.Verify(x => x.Set<Usercomment>().Add(It.IsAny<Usercomment>()), Times.Once);
            created.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_DeletedUserComment()
        {
            var deleted = false;
            var userComments = fixture.GetRandomData(1);

            mockContext.Setup(x => x.Set<Usercomment>().Remove(It.IsAny<Usercomment>())).Callback(() =>
            {
                deleted = true;
            });

            repositoryManager.UserComments.DeleteUserComment(userComments.First());

            mockContext.Verify(x => x.Set<Usercomment>().Remove(It.IsAny<Usercomment>()), Times.Once);
            deleted.Should().BeTrue();
        }
    }
}
