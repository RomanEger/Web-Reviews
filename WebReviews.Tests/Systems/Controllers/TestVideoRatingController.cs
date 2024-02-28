using AutoMapper;
using Entities.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Controllers
{
    public class TestVideoRatingController
    {
        private Mock<IServiceManager> mockServiceManager;
        private VideoRatingController videoRatingController;
        private IMapper mapper;
        private VideoFixture videoFixture;
        private VideoRatingsFixture videoRatingFixture;

        public TestVideoRatingController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            videoRatingController = new VideoRatingController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            videoFixture = new VideoFixture();
            videoRatingFixture = new VideoRatingsFixture(); 
        }

        [Fact]
        public async Task Get_OnSuccess_WriteCorrect_Arguments_WhenCreated()
        {
            var videoRating = videoRatingFixture.GetRandomData(1).First();
            var video = videoFixture.GetRandomData(1).First();

            var videoRatingForManipulation = mapper.Map<VideoRatingForManipulationDTO>(videoRating);
            var videoDTO = mapper.Map<VideoDTO>(video);

            mockServiceManager.Setup(x => x.VideoRating.CreateOrUpdateVideoRatingAsync(videoRatingForManipulation, true))
                .Returns(Task.FromResult(videoDTO));

            var result = await videoRatingController.CreateOrUpdateVideoRating(videoRatingForManipulation);
            var okResult = result as OkObjectResult;
            var returnedVideo = okResult.Value as VideoDTO;

            videoDTO.Should().BeEquivalentTo(returnedVideo);
        }

        [Fact]
        public async Task Get_OnSuccess_RefreshedVideo_Rating()
        {
            var videoRating = videoRatingFixture.GetRandomData(1).First();
            var video = videoFixture.GetRandomData(1).First();

            var videoRatingForManipulation = mapper.Map<VideoRatingForManipulationDTO>(videoRating);
            var videoDTO = mapper.Map<VideoDTO>(video);

            mockServiceManager.Setup(x => x.VideoRating.RefreshVideoRatingAsync(videoDTO.VideoId, true))
                .Returns(Task.FromResult(videoDTO));

            var result = await videoRatingController.RefreshVideoRating(videoDTO.VideoId);
            var okResult = result as OkObjectResult;
            var returnedVideo = okResult.Value as VideoDTO;

            videoDTO.Should().BeEquivalentTo(returnedVideo);
        }
    }
}
