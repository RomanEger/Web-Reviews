using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVideoRatingRepository
    {
        Task<IEnumerable<Videorating>> GetVideoRatingsAsync(Guid videoId, bool trackChanges);
        Task<Videorating> GetVideoRatingAsync(Guid videoId, Guid userId, bool trackChanges);
        void DeleteVideoRating(Videorating videoRating);
        void CreatedVideoRating(Videorating videoRating);
    }
}
