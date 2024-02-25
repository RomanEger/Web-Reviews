using Bogus;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReviews.Tests.Fixtures
{
    public class CommentsFixture : IFixture<Usercomment>
    {
        public Faker<Usercomment> GetUserCommentsRules()
            => new Faker<Usercomment>()
            .RuleFor(x => x.Text, r => r.Random.Words(10))
            .RuleFor(x => x.Advantages, r => r.Random.Words(10))
            .RuleFor(x => x.Disadvantages, r => r.Random.Words(10))
            .RuleFor(x => x.CommentDate, r => r.Date.Recent())
            .RuleFor(x => x.UserId, r => r.Random.Uuid())
            .RuleFor(x => x.VideoId, r => r.Random.Uuid());
        public IEnumerable<Usercomment> GetRandomData(int count) =>
            GetUserCommentsRules().Generate(count);

        public IEnumerable<Usercomment> GetTestData() =>
            new List<Usercomment>()
            {
                new()
                {
                    UserCommentId = new Guid("31bad2cd-024a-4448-a5a8-4df61e37be10"),
                    Text = "Hello",
                    Advantages = "None",
                    Disadvantages = "None",
                    CommentDate = DateTime.Now,
                    UserId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec"),
                    VideoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c")
                },

                new()
                {
                    UserCommentId = new Guid("6ff1a28e-22da-43d3-b1fe-966add34a555"),
                    Text = "Call",
                    Advantages = "Trash",
                    Disadvantages = "cool",
                    CommentDate = DateTime.Now.AddDays(2),
                    UserId = new Guid("6d395f54-d2ab-4f39-aa0e-cce27734b8ec"),
                    VideoId = new Guid("c65bfef7-f5bd-497c-86c5-1e6aed31202c")
                },

                new()
                {
                    UserCommentId = new Guid("24ed9f7a-6e22-4809-839e-faacc95e9e4b"),
                    Text = "not bad",
                    Advantages = "No cool",
                    Disadvantages = "Bad characters",
                    CommentDate = DateTime.Now,
                    UserId = new Guid("a77fc8c4-254d-4a1e-91a6-cc2084c50149"),
                    VideoId = new Guid("a0f3b4a6-1b7c-4376-a215-94839db1c5fb")
                }
            };
    }
}
