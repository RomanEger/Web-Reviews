using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Helpers;

namespace WebReviews.Tests.Fixtures
{
    public class GenericFixture : MainFixture
    {
        private Faker<Videostatus> GenarationRules() =>
            new Faker<Videostatus>()
            .RuleFor(x => x.Title, r => r.Random.Word());

        public FakeDbSet<Videostatus> GetRandomVideoStatuses(int count)
        {
            GenerationGeneric<Videostatus> generationGeneric;
            generationGeneric = GenarationRules;
            return GetRandomGenerationData<Videostatus>(count, generationGeneric);
        }
    }
}
