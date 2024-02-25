using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserCommentsRepository : RepositoryBase<Usercomment>, IUserCommentsRepository
    {
        public UserCommentsRepository(WebReviewsContext webReviewsContext)
            : base(webReviewsContext)
        {
        }

        public void CreateUserComment(Usercomment usercomment) =>
            Create(usercomment);

        public void DeleteUserComment(Usercomment usercomment) =>
            Delete(usercomment);

        public async Task<Usercomment> GetUserCommentByIdAsync(Guid videoId, Guid commentId, bool trackChanges) =>
            await FindByConditions(x => x.VideoId == videoId && x.UserCommentId  == commentId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<PagedList<Usercomment>> GetUserCommentsAsync(Guid videoId, UserCommentsParameters commentsParameters, bool trackChanges)
        {
            var comments = await FindByConditions(x => x.VideoId == videoId, trackChanges)
                .FilterUserCommentByDate(commentsParameters.DateFiltering)
                .Skip((commentsParameters.PageNumber - 1) * commentsParameters.PageSize)
                .Take(commentsParameters.PageSize)
                .ToListAsync();

            var count = FindByConditions(x => x.VideoId == videoId, trackChanges)
                .Skip((commentsParameters.PageNumber - 1) * commentsParameters.PageSize)
                .Take(commentsParameters.PageSize)
                .Count();
            return new PagedList<Usercomment>(comments, count, commentsParameters.PageNumber, commentsParameters.PageSize);
        }
    }
}
