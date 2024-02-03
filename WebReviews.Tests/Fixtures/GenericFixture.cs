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
                    Title = "Title"
                },

                new()
                {
                    Title = "xnjdf"
                },

                new()
                {
                    Title = "Tidfdtle"
                }
            };
    }
}
