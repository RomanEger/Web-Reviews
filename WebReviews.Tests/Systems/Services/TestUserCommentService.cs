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
using Entities.Models;
using MockQueryable.Moq;
using Shared.RequestFeatures;
using FluentAssertions;
using Shared.DataTransferObjects;

namespace WebReviews.Tests.Systems.Services
{
    public class TestUserCommentService
    {
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private VideoFixture videoFixture;
        private CommentsFixture commentsFixture;
        private UserFixture userFixture;

        public TestUserCommentService()
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
            videoFixture = new VideoFixture();
            commentsFixture = new CommentsFixture();
            userFixture = new UserFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComments_Count_2()
        {
            var expectedCount = 2;
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);

            var result = await serviceManager.UserComment.GetUserCommentsAsync(videoId, new UserCommentsParameters(), false);
            result.comments.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComments_WithUserCommentsParameters_Count_1()
        {
            var expectedCount = 1;
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);

            var result = await serviceManager.UserComment.GetUserCommentsAsync(videoId, new UserCommentsParameters { PageSize =1}, false);
            result.comments.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComment_With_Id()
        {
            var expectedCount = 1;
            var commentId = new Guid("6ff1a28e-22da-43d3-b1fe-966add34a555");
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);

            var result = await serviceManager.UserComment.GetUserCommentById(videoId, commentId, false);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedUserComment()
        {
            var created = false;
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();
            var usersToReturn = userFixture.GetTestData().BuildMock().BuildMockDbSet();

            var userCommentForManipulation = autoMapper.Map<UserCommentForManipulationDTO>(userCommentsToReturn.Object.First());

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>().Add(It.IsAny<Usercomment>())).Callback(() => created = true);
            mockContext.Setup(x => x.Set<User>()).Returns(usersToReturn.Object);

            var result = await serviceManager.UserComment.CreateUserCommentAsync(videoId, userCommentForManipulation);
            result.Should().NotBeNull();
            created.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_DeletedUserComment()
        {
            var deleted = false;
            var commentId = new Guid("6ff1a28e-22da-43d3-b1fe-966add34a555");
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>().Remove(It.IsAny<Usercomment>())).Callback(() => deleted = true);

            await serviceManager.UserComment.DeleteUserCommentAsync(videoId, commentId, trackChanges: false);
            deleted.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_UpdatedUserComment()
        {
            var commentId = new Guid("6ff1a28e-22da-43d3-b1fe-966add34a555");
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var videoToReturn = videoFixture.GetTestData().BuildMock().BuildMockDbSet();
            var userCommentsToReturn = commentsFixture.GetTestData().BuildMock().BuildMockDbSet();
            var usersToReturn = userFixture.GetTestData().BuildMock().BuildMockDbSet();

            var userCommentForManipulation = autoMapper.Map<UserCommentForManipulationDTO>(userCommentsToReturn.Object.First());

            mockContext.Setup(x => x.Set<Video>()).Returns(videoToReturn.Object);
            mockContext.Setup(x => x.Set<Usercomment>()).Returns(userCommentsToReturn.Object);
            mockContext.Setup(x => x.Set<User>()).Returns(usersToReturn.Object);

            var result = await serviceManager.UserComment.UpdateUserCommentAsync(videoId, commentId, userCommentForManipulation, trackChanges: true);
            result.Should().NotBeNull();
            result.Text.Should().BeEquivalentTo("Hello");
        }
    }
}
