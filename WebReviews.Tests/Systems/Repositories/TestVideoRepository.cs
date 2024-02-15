using Contracts;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Fixtures;

namespace WebReviews.Tests.Systems.Repositories
{
    public class TestVideoRepository
    {
        private IRepositoryManager repositoryManager;
        private Mock<WebReviewsContext> mockContext;
        private UserFixture fixture;

        public TestVideoRepository()
        {
            mockContext = new Mock<WebReviewsContext>();
            repositoryManager = new RepositoryManager(mockContext.Object);
            fixture = new UserFixture();
        }
    }
}
