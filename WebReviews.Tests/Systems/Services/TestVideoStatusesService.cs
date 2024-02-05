using AutoMapper;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using Service;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.API.Mapper;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Services
{
    public class TestVideoStatusesService
    {
        [Fact]
        public async Task Get_OnSuccess_Created_Entity_And_Returned_Entity()
        {
            var fixture = new GenericFixture();

            var created = false;
            var videoStatusEntity = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            var referenceDTO = new ReferenceForManipulationDTO { title = "new" };

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var videoStatus = autoMapper.Map<Videostatus>(referenceDTO);

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>().Add(It.IsAny<Videostatus>())).Callback(() =>
            {
                created = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);

            
            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            await serviceManager.VideoStatuses.CreateVideoStatusAsync(referenceDTO);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<Videostatus>().Add(It.IsAny<Videostatus>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_VideoStatus_With_Id()
        {
            var fixture = new GenericFixture();


            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();

            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();


            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var entity = await serviceManager.VideoStatuses.GetVideoStatusByIdAsync(guid, trackChanges: true);

            entity.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_List_Of_VideoStatuses()
        {
            var fixture = new GenericFixture();

            var expectedCount = 3;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();



            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();


            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var entities = await serviceManager.VideoStatuses.GetVideoStatusesAsync(trackChanges: true);

            entities.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_Updated_VideoStatus_WithTitle_Updated()
        {
            var fixture = new GenericFixture();

            var expectedCount = 3;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");
            var referenceForManipulation = new ReferenceForManipulationDTO { title = "Updated" };


            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();


            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            var entitiy = await serviceManager.VideoStatuses.UpdateVideoStatus(guid, referenceForManipulation, trackChanges: true);

            entitiy.Should().NotBeNull();
            entitiy.title.Should().BeEquivalentTo(referenceForManipulation.title);
        }

        [Fact]
        public async Task Get_OnSuccess_Deleted_VideoStatus_ById()
        {
            var fixture = new GenericFixture();

            var deleted = false;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");


            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });

            var autoMapper = mockAutoMapper.CreateMapper();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);
            mockContext.Setup(x => x.Set<Videostatus>().Remove(It.IsAny<Videostatus>())).Callback(() =>
            {
                deleted = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);


            var serviceManager = new ServiceManager(repositoryManager, autoMapper);

            await serviceManager.VideoStatuses.DeleteVideoStatusAsync(guid, trackChanges:true);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<Videostatus>().Remove(It.IsAny<Videostatus>()), Times.Once());
        }
    }
}
