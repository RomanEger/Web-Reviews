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
        public async Task Get_OnSucces_IEnumerable_VideoStatus_With_Count_5()
        {
            var fixture = new GenericFixture();

            var expectedCount = 1;
            var rndList = new List<Videostatus> { new Videostatus { Title = "Oleg" } };
            var listOfVideoStatus = rndList.BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>()).Returns(listOfVideoStatus.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var list = await repositoryManager.VideoStatuses.GetAllAsync(trackChanges: false);

            list.Should().HaveCount(expectedCount);
        }
    }
}
