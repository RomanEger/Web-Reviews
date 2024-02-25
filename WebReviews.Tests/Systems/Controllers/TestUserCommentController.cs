using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Controllers
{
    public class TestUserCommentController
    {
        private Mock<IServiceManager> mockServiceManager;
        private UserCommentController userCommentController;
        private IMapper mapper;
        private CommentsFixture fixture;

        public TestUserCommentController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            userCommentController = new UserCommentController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new CommentsFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComments_Count_2()
        {
            var expectedCount = 2;
            var userComments = fixture.GetRandomData(expectedCount);
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var userCommentsDTO = mapper.Map<IEnumerable<UserCommentDTO>>(userComments);
            
            mockServiceManager.Setup(x => x.UserComment.GetUserCommentsAsync(videoId, It.IsAny<UserCommentsParameters>(), false))
                .Returns(Task.FromResult((userCommentsDTO, new MetaData())));

            await userCommentController.Invoking(async x => await x.GetUserComments(videoId, new UserCommentsParameters { }))
                .Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnedUserComment()
        {
            var userComments = fixture.GetRandomData(1).First();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var commentId = Guid.NewGuid();
            var userCommentDTO = mapper.Map<UserCommentDTO>(userComments);

            mockServiceManager.Setup(x => x.UserComment.GetUserCommentById(videoId, commentId, false))
                .Returns(Task.FromResult(userCommentDTO));

            var result = await userCommentController.GetUserComment(videoId, commentId);
            var okResult = result as OkObjectResult;
            var values = okResult.Value as UserCommentDTO;
            values.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedUserComment()
        {
            var userComments = fixture.GetRandomData(1).First();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var commentId = Guid.NewGuid();
            var userCommentDTO = mapper.Map<UserCommentDTO>(userComments);

            mockServiceManager.Setup(x => x.UserComment.CreateUserCommentAsync(videoId, It.IsAny<UserCommentForManipulationDTO>()))
                .Returns(Task.FromResult(userCommentDTO));

            var result = await userCommentController.CreateUserComment(videoId, new UserCommentForManipulationDTO());
            
            mockServiceManager.Verify(x => x.UserComment.CreateUserCommentAsync(videoId, It.IsAny<UserCommentForManipulationDTO>()), Times.Once);
        }

        [Fact]
        public async Task Get_OnSuccess_DeletedUserComment()
        {
            var deleted = false;
            var userComments = fixture.GetRandomData(1).First();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var commentId = Guid.NewGuid();
            var userCommentDTO = mapper.Map<UserCommentDTO>(userComments);

            mockServiceManager.Setup(x => x.UserComment.DeleteUserCommentAsync(videoId, commentId, false))
                .Callback(() =>  deleted = true);

            var result = await userCommentController.DeleteUserComment(videoId, commentId);

            deleted.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_UpdatedUserComment()
        {
            var updated = false;
            var userComments = fixture.GetRandomData(1).First();
            var videoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c");
            var commentId = Guid.NewGuid();
            var userCommentDTO = mapper.Map<UserCommentDTO>(userComments);

            mockServiceManager.Setup(x => x.UserComment.UpdateUserCommentAsync(videoId, commentId, It.IsAny<UserCommentForManipulationDTO>(), true))
                .Callback(() => updated = true);

            var result = await userCommentController.UpdateUserComment(videoId, commentId, new UserCommentForManipulationDTO());

            updated.Should().BeTrue();
        }
    }
}
