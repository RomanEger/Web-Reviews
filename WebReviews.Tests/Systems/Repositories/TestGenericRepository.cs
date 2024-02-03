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
        [Fact]
        public async Task Get_OnSucces_IEnumerable_VideoStatuses_With_Count_5()
        {
            var fixture = new GenericFixture();

            var expectedCount = 4;
            var listOfVideoStatus = fixture.GetRandomData(expectedCount).BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(listOfVideoStatus.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var list = await repositoryManager.VideoStatuses.GetAllAsync(trackChanges: false);

            list.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSucces_IEnumerable_VideoStatuses_With_Count_1_By_Condition()
        {
            var fixture = new GenericFixture();

            var listOfVideoStatus = fixture.GetTestData().BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(listOfVideoStatus.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var entity = await repositoryManager
                .VideoStatuses
                .GetGyConditionAsync(x => x.Title.StartsWith("Tit", StringComparison.InvariantCultureIgnoreCase),trackChanges: false);

            entity.Should().BeOfType<Videostatus>();
        }

        [Fact]
        public async Task Get_OnSucces_Delete_VideoStatus()
        {
            var fixture = new GenericFixture();

            var deleted = false;
            var listOfVideoStatus = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>().Remove(listOfVideoStatus.Object.First())).Callback(() =>
            {
                deleted = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);

            repositoryManager.VideoStatuses.DeleteEntity(listOfVideoStatus.Object.First());

            deleted.Should().BeTrue();
        }

        [Fact]
        public async Task Get_OnSucces_Create_VideoStatus()
        {
            var fixture = new GenericFixture();

            var created = false;
            var listOfVideoStatus = fixture.GetRandomData(1).BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>().Add(listOfVideoStatus.Object.First())).Callback(() =>
            {
                created = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);

            repositoryManager.VideoStatuses.CreateEntity(listOfVideoStatus.Object.First());

            created.Should().BeTrue();
        }
    }
}
