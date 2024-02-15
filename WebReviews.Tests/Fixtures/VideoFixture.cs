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

        public IEnumerable<Video> GetRandomData(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Video> GetTestData()
        {
            throw new NotImplementedException();
        }
    }
}
