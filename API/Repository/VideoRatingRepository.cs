using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VideoRatingRepository : RepositoryBase<Videorating>, IVideoRatingRepository
    {
        public VideoRatingRepository(WebReviewsContext webReviewsContext) 
            : base(webReviewsContext)
        {
        }

        public void CreateVideoRating(Videorating videoRating) =>
            Create(videoRating);

        public void DeleteVideoRating(Videorating videoRating) =>
            Delete(videoRating);

        public async Task<Videorating> GetVideoRatingAsync(Guid videoId, Guid userId, bool trackChanges) =>
            await FindByConditions(x => x.VideoId == videoId && x.UserId == userId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Videorating>> GetVideoRatingsAsync(Guid videoId, bool trackChanges) =>
            await FindByConditions(x => x.VideoId == videoId, trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Videorating>> GetUserRatingsAsync(Guid userId, bool trackChanges) =>
            await FindByConditions(x => x.UserId == userId, trackChanges)
                .ToListAsync();
    }
}
