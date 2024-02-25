using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserCommentService
    {
        Task<(IEnumerable<UserCommentDTO> comments, MetaData metaData)> GetUserCommentsAsync(Guid videoId,
                                                                                             UserCommentsParameters commentsParameters,
                                                                                             bool trackChanges);
        Task<UserCommentDTO> GetUserCommentById(Guid videoId, Guid commentId, bool trackChanges);

        Task<UserCommentDTO> CreateUserCommentAsync(Guid videoId,UserCommentForManipulationDTO commentForManipulation);
        Task DeleteUserCommentAsync(Guid videoId, Guid commentId, bool trackChanges);
        Task<UserCommentDTO> UpdateUserCommentAsync(Guid videoId,
                                                    Guid commentId,
                                                    UserCommentForManipulationDTO commentForManipulation,
                                                    bool trackChanges);
    }
}
