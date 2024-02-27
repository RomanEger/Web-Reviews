using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReviews.Tests.Fixtures
{
    public class VideoRatingsFixture : IFixture<Videorating>
    {
        public Faker<Videorating> GetVideoRatingRules() =>
            new Faker<Videorating>()
            .RuleFor(x => x.Rating, r => r.Random.Number(1, 10))
            .RuleFor(x => x.UserId, r => r.Random.Uuid())
            .RuleFor(x => x.VideoId, r => r.Random.Uuid());

        public IEnumerable<Videorating> GetRandomData(int count) =>
            GetVideoRatingRules().Generate(count);

        public IEnumerable<Videorating> GetTestData() =>
            new List<Videorating>()
            {
                new()
                {
                    Rating = 3,
                    UserId = new Guid("92a913d4-84b2-487d-8ee9-27f5ae0fdef2"),
                    VideoId = new Guid("6e090bc7-6d8c-4758-acf8-b729473626b6")
                },
                new()
                {
                    Rating = 8,
                    UserId = new Guid("c901d089-3693-4cd5-8305-b2386383afbb"),
                    VideoId = new Guid("0bbcde5b-8e3d-42a5-9860-3d2114ada40e")
                },
                new()
                {
                    Rating = 4,
                    UserId = new Guid("7abd6ca5-219e-4254-a26a-e718d0d64d98"),
                    VideoId = new Guid("0bbcde5b-8e3d-42a5-9860-3d2114ada40e")
                }
            };
    }
}
