using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public VideoRepository(WebReviewsContext webReviewsContext)
            : base(webReviewsContext)
        {
        }

        public void CreateVideo(Video video) =>
            Create(video);

        public void DeleteVideo(Video video) => 
            Delete(video);

        public async Task<Video> GetVideoAsync(Guid videoId, bool trackChanges) =>
            await FindByConditions(x => x.VideoId == videoId, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<PagedList<Video>> GetVideosAsync(VideoParameters videoParameters, bool trackChanges)
        {
            var listOfVideos = await FindAll(trackChanges)
                .Skip((videoParameters.PageNumber - 1) * videoParameters.PageSize)
                .Take(videoParameters.PageSize)
                .ToListAsync();
            var count = FindAll(trackChanges)
                .Count();
            return new PagedList<Video>(listOfVideos, count, videoParameters.PageNumber, videoParameters.PageSize);
        }
    }
}
