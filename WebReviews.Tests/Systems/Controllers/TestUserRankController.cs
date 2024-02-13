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
    public class TestUserRankController
    {
        private Mock<IServiceManager> mockServiceManager;
        private UserRankController userRankController;
        private IMapper mapper;

        public TestUserRankController()
        {
            mockServiceManager = new Mock<IServiceManager>();
            userRankController = new UserRankController(mockServiceManager.Object);

            var mockAutoMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            mapper = mockAutoMapper.CreateMapper();
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_UserRanks_Count_2()
        {
            var expectedCount = 2;

            var UserRanks = new List<Userrank>
            {
                new()
                {
                    UserRankId = Guid.NewGuid(),
                    Title = "Test",
                    Description = "Test"
                },

                new()
                {
                    UserRankId = Guid.NewGuid(),
                    Title = "Test",
                    Description = "Test"
                }
            };

            var userRanksDTO = mapper.Map<IEnumerable<ExtentedReferenceDTO>>(UserRanks);

            mockServiceManager.Setup(x => x.UserRank.GetUserRanksAsync(It.IsAny<bool>())).Returns(Task.FromResult(userRanksDTO));

            var result = await userRankController.GetUserRanks();

            var okResult = result as OkObjectResult;
            var returnedUserRanks = okResult.Value as IEnumerable<ExtentedReferenceDTO>;

            returnedUserRanks.Should().HaveCount(expectedCount);
        }

        [Fact]
        public async Task Get_OnSuccess_Returned_UserRank_With_Id()
        {
            var guid = Guid.NewGuid();
            var UserRanks = new Userrank
            {
                UserRankId = Guid.NewGuid(),
                Title = "Test",
                Description = "Test"
            };           

            var userRankDTO = mapper.Map<ExtentedReferenceDTO>(UserRanks);

            mockServiceManager.Setup(x => x.UserRank.GetUserRankAsync(guid, It.IsAny<bool>())).Returns(Task.FromResult(userRankDTO));

            var result = await userRankController.GetUserRank(guid);

            var okResult = result as OkObjectResult;
            var returnedUserRank = okResult.Value as ExtentedReferenceDTO;

            returnedUserRank.Should().NotBeNull();
        }
    }
}
