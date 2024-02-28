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
                    UserId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec"),
                    VideoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c")
                },
                new()
                {
                    Rating = 8,
                    UserId = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403"),
                    VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb")
                },
                new()
                {
                    Rating = 4,
                    UserId = new Guid("45278281-eaa7-447a-abb5-4e705ff6b613"),
                    VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb")
                }
            };
    }
}
