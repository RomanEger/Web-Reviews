using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<Videostatus>().Add(videoStatusEntity.Object.First())).Callback(() =>
            {
                created = true;
            });

            var repositoryManager = new ServiceManager(mockContext.Object);

            repositoryManager.VideoStatuses.CreateEntity(videoStatusEntity.Object.First());

            created.Should().BeTrue();
        }
    }
}
