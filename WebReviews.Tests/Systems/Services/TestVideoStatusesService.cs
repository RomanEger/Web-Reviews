using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Repository;
using Service;
using Service.Contracts;
using Service.Helpers;
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
        private Mock<WebReviewsContext> mockContext;
        private IMapper autoMapper;
        private IRepositoryManager repositoryManager;
        private EntityChecker entityChecker;
        private IOptions<JwtConfiguration> options;
        private IServiceManager serviceManager;
        private GenericFixture fixture;
        public TestVideoStatusesService()
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
            fixture = new GenericFixture();
        }
        [Fact]
        public async Task Get_OnSuccess_Created_Entity_And_Returned_Entity()
        {
            var created = false;
            var videoStatusEntity = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            var referenceDTO = new ReferenceForManipulationDTO { title = "new" };

            mockContext.Setup(x => x.Set<Videostatus>().Add(It.IsAny<Videostatus>())).Callback(() =>
            {
                created = true;
            });

            await serviceManager.VideoStatuses.CreateVideoStatusAsync(referenceDTO);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<Videostatus>().Add(It.IsAny<Videostatus>()), Times.Once());
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_VideoStatus_With_Id()
        {
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();

            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);


            var entity = await serviceManager.VideoStatuses.GetVideoStatusByIdAsync(guid, trackChanges: true);

            entity.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_List_Of_VideoStatuses()
        {
            var expectedCount = 3;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);


            var entities = await serviceManager.VideoStatuses.GetVideoStatusesAsync(trackChanges: true);

            entities.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_Updated_VideoStatus_WithTitle_Updated()
        {
            var expectedCount = 3;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");
            var referenceForManipulation = new ReferenceForManipulationDTO { title = "Updated" };

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            var entitiy = await serviceManager.VideoStatuses.UpdateVideoStatus(guid, referenceForManipulation, trackChanges: true);

            entitiy.Should().NotBeNull();
            entitiy.title.Should().BeEquivalentTo(referenceForManipulation.title);
            entitiy.Should().BeOfType(typeof(ReferenceDTO));
        }

        [Fact]
        public async Task Get_OnSuccess_Deleted_VideoStatus_ById()
        {
            var deleted = false;
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d");


            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);
            mockContext.Setup(x => x.Set<Videostatus>().Remove(It.IsAny<Videostatus>())).Callback(() =>
            {
                deleted = true;
            });


            await serviceManager.VideoStatuses.DeleteVideoStatusAsync(guid, trackChanges:true);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<Videostatus>().Remove(It.IsAny<Videostatus>()), Times.Once());
        }

        [Fact]
        public async Task Get_Failed_Update_With_Incorrect_Id_Returned_NotFoundException()
        {
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2433cf03-6d26-42db-81d2-78064a44f43d");
            var referenceForManipulation = new ReferenceForManipulationDTO { title = "Updated" };

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            await serviceManager.Invoking(async c => await c.VideoStatuses.UpdateVideoStatus(guid, referenceForManipulation, trackChanges: true))
                .Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task Get_Failed_With_GetById_Incorrect_Id_Returned_NotFoundException()
        {
            var videoStatusEntities = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("2433cf03-6d26-42db-81d2-78064a44f43d");

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(videoStatusEntities.Object);

            await serviceManager.Invoking(async c => await c.VideoStatuses.GetVideoStatusByIdAsync(guid, trackChanges: true))
                .Should().ThrowAsync<NotFoundException>();
        }
    }
}
