using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MockQueryable.Moq;
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
    public class TestVideoStatusController
    {
        private Mock<IServiceManager> mockServiceManager;
        private VideoStatusController videoStatusController;
        private IMapper mapper;
        private GenericFixture fixture;

        public TestVideoStatusController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            videoStatusController = new VideoStatusController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
            fixture = new GenericFixture();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_ListOf_VideoStatuses_Count_5()
        {
            var expectedCount = 5;
            var listOfVideoStatuse = fixture.GetRandomData(expectedCount);

            var listOfVideoStatusesDTO = mapper.Map<IEnumerable<ReferenceDTO>>(listOfVideoStatuse);

            mockServiceManager.Setup(x => x.VideoStatuses.GetVideoStatusesAsync(It.IsAny<bool>())).Returns(Task.FromResult(listOfVideoStatusesDTO));

            var result = await videoStatusController.GetVideoStatuses();
            var okResult = result as OkObjectResult;
            var returnedList = okResult.Value as List<ReferenceDTO>;

            returnedList.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_VideoStatus_WithId()
        {
            var videoStatus = fixture.GetTestData().First();

            var videoStatusDTO = mapper.Map<ReferenceDTO>(videoStatus);

            mockServiceManager.Setup(x => x.VideoStatuses.GetVideoStatusByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(videoStatusDTO));

            var result = await videoStatusController.GetVideoStatus(Guid.NewGuid());
            var okResult = result as OkObjectResult;
            var returnedEntity = okResult.Value as ReferenceDTO;

            returnedEntity.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Updated_VideoStatus()
        {
            var videoStatus = fixture.GetTestData().First();

            var videoStatusDTO = mapper.Map<ReferenceDTO>(videoStatus);
            var videoStatusForManipDTO = mapper.Map<ReferenceForManipulationDTO>(videoStatus);

            mockServiceManager.Setup(x => x.VideoStatuses.UpdateVideoStatus(It.IsAny<Guid>(),
                It.IsAny<ReferenceForManipulationDTO>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(videoStatusDTO));

            var result = await videoStatusController.UpdateVideoStatus(videoStatusForManipDTO ,Guid.NewGuid());
            var okResult = result as OkObjectResult;
            var returnedEntity = okResult.Value as ReferenceDTO;

            returnedEntity.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Created_VideoStatus()
        {
            var videoStatus = fixture.GetTestData().First();

            var videoStatusDTO = mapper.Map<ReferenceDTO>(videoStatus);
            var videoStatusForManipDTO = mapper.Map<ReferenceForManipulationDTO>(videoStatus);

            mockServiceManager.Setup(x => x.VideoStatuses.CreateVideoStatusAsync(It.IsAny<ReferenceForManipulationDTO>()))
                .Returns(Task.FromResult(videoStatusDTO));
            var result = await videoStatusController.CreateVideoStatus(videoStatusForManipDTO);

            mockServiceManager.Verify(x => x.VideoStatuses.CreateVideoStatusAsync(It.IsAny<ReferenceForManipulationDTO>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Deleted_VideoStatus()
        {
            var videoStatus = fixture.GetTestData().First();

            var videoStatusDTO = mapper.Map<ReferenceDTO>(videoStatus);
            var videoStatusForManipDTO = mapper.Map<ReferenceForManipulationDTO>(videoStatus);

            mockServiceManager.Setup(x => x.VideoStatuses.DeleteVideoStatusAsync(It.IsAny<Guid>(), It.IsAny<bool>()));

            var result = await videoStatusController.DeleteVideoStatus(Guid.NewGuid());

            mockServiceManager.Verify(x => x.VideoStatuses.DeleteVideoStatusAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Once());
        }
    }
}
