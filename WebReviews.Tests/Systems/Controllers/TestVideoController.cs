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
    public class TestVideoController
    {
        private Mock<IServiceManager> mockServiceManager;
        private VideoController videoController;
        private IMapper mapper;
        private VideoFixture fixture;

        public TestVideoController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            videoController = new VideoController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new VideoFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_ListOfVideos()
        {
            var expectedCount = 5;
            var videos = fixture.GetRandomData(expectedCount);

            var videosDTO = mapper.Map<IEnumerable<VideoDTO>>(videos);
            var videoParameters = new VideoParameters();

            mockServiceManager.Setup(x => x.Video.GetVideosAsync(
                It.IsAny<VideoParameters>(),
                It.IsAny<bool>())).Returns(Task.FromResult((videosDTO, new MetaData())));

            await videoController.Invoking(async x => await x.GetVideos(videoParameters)).Should().ThrowAsync<NullReferenceException>();
        }

        [Fact]
        public async Task Get_OnSuccess_VideoWithId()
        {
            var video = fixture.GetRandomData(1).First();

            var videoDTO = mapper.Map<VideoDTO>(video);

            mockServiceManager.Setup(x => x.Video.GetVideoByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<bool>())).Returns(Task.FromResult(videoDTO));

            var resultUser = await videoController.GetVideo(Guid.NewGuid());

            var okResult = resultUser as OkObjectResult;
            var returnedVideo = okResult.Value as VideoDTO;

            returnedVideo.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_CreatedVideo()
        {
            var video = fixture.GetRandomData(1).First();

            var videoDTO = mapper.Map<VideoDTO>(video);
            var videoForManipulation = mapper.Map<VideoForManipulationDTO>(video);

            mockServiceManager.Setup(x => x.Video.CreateVideoAsync(
                It.IsAny<VideoForManipulationDTO>())).Returns(Task.FromResult(videoDTO));

            var resultUser = await videoController.CreateVideo(videoForManipulation);

            mockServiceManager.Verify(x => x.Video.CreateVideoAsync(videoForManipulation), Times.Once);
        }

        [Fact]
        public async Task Get_OnSuccess_DeletedVideo()
        {
            var deleted = false;

            mockServiceManager.Setup(x => x.Video.DeleteVideoAsync(
                It.IsAny<Guid>(), It.IsAny<bool>()))
                .Callback(() =>
                {
                    deleted = true;
                });

            var resultUser = await videoController.DeleteVideo(Guid.NewGuid());

            mockServiceManager.Verify(x => x.Video.DeleteVideoAsync(It.IsAny<Guid>(), false), Times.Once);
            deleted.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSuccess_UpdatedVideo()
        {
            var video = fixture.GetRandomData(1).First();

            var videoDTO = mapper.Map<VideoDTO>(video);
            var videoForManipulation = mapper.Map<VideoForManipulationDTO>(video);

            mockServiceManager.Setup(x => x.Video.UpdateVideoAsync(
                It.IsAny<Guid>(), videoForManipulation, It.IsAny<bool>())).Returns(Task.FromResult(videoDTO));

            var resultUser = await videoController.UpdateVideo(videoForManipulation, Guid.NewGuid());

            mockServiceManager.Verify(x => x.Video.UpdateVideoAsync(It.IsAny<Guid>(), videoForManipulation,true), Times.Once);
        }
    }
}
