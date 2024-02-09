using Entities.Models;
using FluentAssertions;
using MockQueryable.Moq;
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
    public class TestUserRepository
    {
        [Fact]
        public async Task Get_OnSucces_IEnumerable_Users_With_Count_4()
        {
            var fixture = new UserFixture();

            var expectedCount = 4;
            var listOfUsers = fixture.GetRandomData(expectedCount).BuildMock().BuildMockDbSet();

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(listOfUsers.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var list = await repositoryManager.User.GetUsersAsync(trackChanges: false);

            list.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSucces_User_With_Id()
        {
            var fixture = new UserFixture();

            var listOfUsers = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var guid = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec");

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(listOfUsers.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var user = await repositoryManager.User.GetUserAsync(guid, trackChanges: false);

            user.Should().NotBeNull();
            user.Nickname.Should().Be("MakkLaud");
        }

        [Fact]
        public async Task Get_OnSucces_User_With_Nickname_Roman()
        {
            var fixture = new UserFixture();

            var listOfUsers = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var nickname = "Roman";

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(listOfUsers.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var user = await repositoryManager.User.GetUserByNicknameAsync(nickname, trackChanges: false);

            user.Should().NotBeNull();
            user.Nickname.Should().Be(nickname);
        }

        [Fact]
        public async Task Get_OnSucces_User_With_Empty_Nickname()
        {
            var fixture = new UserFixture();

            var listOfUsers = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var nickname = String.Empty;

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(listOfUsers.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var user = await repositoryManager.User.GetUserByNicknameAsync(nickname, trackChanges: false);

            user.Should().BeNull();
        }

        [Fact]
        public async Task Get_OnSucces_User_With_Email()
        {
            var fixture = new UserFixture();

            var listOfUsers = fixture.GetTestData().BuildMock().BuildMockDbSet();
            var email = "andrew@mail.ru";

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>()).Returns(listOfUsers.Object);

            var repositoryManager = new RepositoryManager(mockContext.Object);

            var user = await repositoryManager.User.GetUserByEmailAsync(email, trackChanges: false);

            user.Should().NotBeNull();
            user.Email.Should().Be(email);
        }

        [Fact]
        public async Task Delete_OnSucces_User()
        {
            var fixture = new UserFixture();

            var user = fixture.GetRandomData(1).BuildMock().BuildMockDbSet().Object.First();
            var deleted = false;

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>().Remove(It.IsAny<User>())).Callback(() =>
            {
                deleted = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);

            repositoryManager.User.DeleteUser(user);

            deleted.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Remove(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Create_OnSucces_User()
        {
            var fixture = new UserFixture();

            var user = fixture.GetRandomData(1).BuildMock().BuildMockDbSet().Object.First();
            var created = false;

            var mockContext = new Mock<WebReviewsContext>();
            mockContext.Setup(x => x.Set<User>().Add(It.IsAny<User>())).Callback(() =>
            {
                created = true;
            });

            var repositoryManager = new RepositoryManager(mockContext.Object);

            repositoryManager.User.CreateUser(user);

            created.Should().BeTrue();
            mockContext.Verify(x => x.Set<User>().Add(It.IsAny<User>()), Times.Once);
        }
    }
}
