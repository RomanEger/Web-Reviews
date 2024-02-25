using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserCommentsRepository
    {
        Task<PagedList<Usercomment>> GetUserCommentsAsync(Guid videoId, UserCommentsParameters commentsParameters, bool trackChanges);
        Task<Usercomment> GetUserCommentByIdAsync(Guid videoId, Guid commentId, bool trackChanges);
        void DeleteUserComment(Usercomment usercomment);
        void CreateUserComment(Usercomment usercomment);
    }
}
