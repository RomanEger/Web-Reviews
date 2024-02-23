using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReviews.Tests.Fixtures
{
    public class VideoFixture : IFixture<Video>
    {
        private Faker<Video> GetVideoRules() =>
            new Faker<Video>()
            .RuleFor(x => x.Title, r => r.Random.Word())
            .RuleFor(x => x.VideoTypeId, r => r.Random.Guid())
            .RuleFor(x => x.VideoStatusId, r => r.Random.Guid())
            .RuleFor(x => x.VideoRestrictionId, r => r.Random.Guid())
            .RuleFor(x => x.Description, r => r.Random.Words(5))
            .RuleFor(x => x.CurrentEpisode, r => r.Random.Number(1, 24))
            .RuleFor(x => x.TotalEpisodes, r => r.Random.Number(12, 100))
            .RuleFor(x => x.ReleaseDate, r => r.Date.Recent())
            .RuleFor(x => x.Photo, r => r.Random.Bytes(24))
            .RuleFor(x => x.Rating, r => r.Random.Decimal(min: 1, max: 10));            

        public IEnumerable<Video> GetRandomData(int count) =>
            GetVideoRules().Generate(count);

        public IEnumerable<Video> GetTestData() =>
            new List<Video>()
            {
                new()
                {
                    VideoId = new Guid("c3558440-c0de-48ee-afd9-ffc6d5b70fa9"),
                    Title = "Demon Slayer",
                    VideoTypeId = new Guid("40aaad63-0371-4359-b986-18eb9ee1c47b"),
                    VideoStatusId = new Guid("8f2ab181-a87e-4a90-89b2-65f6cffd3431"),
                    VideoRestrictionId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c"),
                    Description = null,
                    CurrentEpisode = 24,
                    TotalEpisodes = 24,
                    ReleaseDate = DateTime.Now.AddDays(15),
                    Photo = null,
                    Rating = (decimal?)9.2
                },

                new()
                {
                    VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb"),
                    Title = "Dunter x Hunter",
                    VideoTypeId = new Guid("40aaad63-0371-4359-b986-18eb9ee1c47b"),
                    VideoStatusId = new Guid("8f2ab181-a87e-4a90-89b2-65f6cffd3431"),
                    VideoRestrictionId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c"),
                    Description = null,
                    CurrentEpisode = 12,
                    TotalEpisodes = 24,
                    ReleaseDate = DateTime.Now.AddDays(4),
                    Photo = null,
                    Rating = (decimal?)8.4
                },

                new()
                {
                    VideoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c"),
                    Title = "Bakuman",
                    VideoTypeId = new Guid("40aaad63-0371-4359-b986-18eb9ee1c47b"),
                    VideoStatusId = new Guid("8f2ab181-a87e-4a90-89b2-65f6cffd3431"),
                    VideoRestrictionId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c"),
                    Description = null,
                    CurrentEpisode = 22,
                    TotalEpisodes = 48,
                    ReleaseDate = DateTime.Now.AddDays(10),
                    Photo = null,
                    Rating = (decimal?)9.9
                },
            };
    }
}
