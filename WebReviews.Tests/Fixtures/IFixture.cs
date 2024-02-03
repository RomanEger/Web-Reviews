using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebReviews.Tests.Fixtures
{
    public interface IFixture<T> where T : class
    {
        IEnumerable<T> GetRandomData(int count);

        IEnumerable<T> GetTestData();
    }
}
