using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReviews.Tests.Helpers;

namespace WebReviews.Tests.Fixtures
{
    public abstract class MainFixture
    {
        protected delegate Faker<T> GenerationGeneric<T>() where T : class;

        protected FakeDbSet<T> GetRandomGenerationData<T>(int count, GenerationGeneric<T> generationGeneric) where T : class
        {
            var listRandomData = generationGeneric().Generate(count);
            FakeDbSet<T> values = [.. listRandomData];
            return values;
        }
    }
}
