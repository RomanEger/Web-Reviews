using Contracts;
using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Repositories
{
    public class TestGenericRepository
    {

        private IRepositoryManager repositoryManager;
        private Mock<WebReviewsContext> mockContext;
        private GenericFixture fixture;

        public TestGenericRepository()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            fixture = new GenericFixture();
        }

        [Fact]
        public async Task Get_OnSucces_IEnumerable_VideoStatuses_With_Count_5()
        {
            var expectedCount = 4;
            var listOfVideoStatus = fixture.GetRandomData(expectedCount).BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(listOfVideoStatus.Object);

            var list = await repositoryManager.VideoStatuses.GetAllAsync(trackChanges: false);

            list.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSucces_IEnumerable_VideoStatuses_With_Count_1_By_Condition()
        {
            var listOfVideoStatus = fixture.GetTestData().BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videostatus>()).Returns(listOfVideoStatus.Object);

            var entity = await repositoryManager
                .VideoStatuses
                .GetGyConditionAsync(x => x.Title.StartsWith("Tit", StringComparison.InvariantCultureIgnoreCase),trackChanges: false);

            entity.Should().BeOfType<Videostatus>();
        }

        [Fact]
        public void Get_OnSucces_Delete_VideoStatus()
        {

            var deleted = false;
            var VideoStatusEntity = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videostatus>().Remove(VideoStatusEntity.Object.First())).Callback(() =>
            {
                deleted = true;
            });

            repositoryManager.VideoStatuses.DeleteEntity(VideoStatusEntity.Object.First());

            deleted.Should().BeTrue();
        }

        [Fact]
        public void Get_OnSucces_Create_VideoStatus()
        {
            var created = false;
            var VideoStatusEntity = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            mockContext.Setup(x => x.Set<Videostatus>().Add(VideoStatusEntity.Object.First())).Callback(() =>
            {
                created = true;
            });

            repositoryManager.VideoStatuses.CreateEntity(VideoStatusEntity.Object.First());

            created.Should().BeTrue();
        }
    }
}
