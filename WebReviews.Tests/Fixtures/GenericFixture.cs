using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebReviews.Tests.Fixtures
{
    public class GenericFixture : IFixture<Videostatus>
    {
        private Faker<Videostatus> GenarationRules() =>
            new Faker<Videostatus>()
            .RuleFor(x => x.VideoStatusId, r => r.Random.Guid())
            .RuleFor(x => x.Title, r => r.Random.Word());

        public IEnumerable<Videostatus> GetRandomData(int count) =>
            GenarationRules().Generate(count);

        public IEnumerable<Videostatus> GetTestData() =>
            new List<Videostatus>
            {
                new()
                {
                    VideoStatusId = new Guid("402768ce-2991-41fe-a82b-c8dbfff89b8d"),
                    Title = "Title"
                },

                new()
                {
                    VideoStatusId = new Guid("2403cf03-6d26-42db-81d2-78064a44f43d"),
                    Title = "xnjdf"
                },

                new()
                {
                    VideoStatusId = new Guid("88e50458-eb8c-4249-9572-b6d3ff0ac9f5"),
                    Title = "Tidfdtle"
                }
            };
    }
}
