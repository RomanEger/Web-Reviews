using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReviews.Tests.Fixtures
{
    public class UserFixture : IFixture<User>
    {
        private Faker<User> GenerationRules() =>
            new Faker<User>()
            .RuleFor(x => x.UserId, r => r.Random.Guid())
            .RuleFor(x => x.Nickname, r => r.Random.Word())
            .RuleFor(x => x.Email, r => r.Internet.Email())
            .RuleFor(x => x.Password, r => r.Internet.Password())
            .RuleFor(x => x.UserRankId, r => r.Random.Guid())
            .RuleFor(x => x.Photo, r => r.Random.Bytes(10));

        public IEnumerable<User> GetRandomData(int count) =>
            GenerationRules().Generate(count);

        public IEnumerable<User> GetTestData() =>
            new List<User>()
            {
                new()
                {
                    UserId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec"),
                    Nickname = "MakkLaud",
                    Email = "MakkLaud@mail.ru",
                    Password = "password",
                    UserRankId = new Guid("7f61426a-bf76-4cb7-824c-8ab3dd4065fd")
                },
                new()
                {
                    UserId = new Guid("08feaf40-ea7f-404d-ade6-b2fb1c009403"),
                    Nickname = "Roman",
                    Email = "roman@mail.ru",
                    Password = "amZna2o0NQ==",
                    UserRankId = new Guid("4c58a463-db9b-4044-b111-f607243691ef")
                },
                new()
                {
                    UserId = new Guid("45278281-eaa7-447a-abb5-4e705ff6b613"),
                    Nickname = "Andrew",
                    Email = "andrew@mail.ru",
                    Password = "none null",
                    UserRankId = new Guid("94371658-dbfa-4a5c-8d86-be0c0cbb6173")
                }
            };
    }
}
