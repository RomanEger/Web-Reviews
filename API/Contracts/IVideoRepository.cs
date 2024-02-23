using Entities.Models;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVideoRepository
    {
        Task<PagedList<Video>> GetVideosAsync(VideoParameters videoParameters, bool trackChanges);
        Task<Video> GetVideoAsync(Guid videoId, bool trackChanges);
        Task<Video> GetVideoByTitle(string title, bool trackChanges);
        void DeleteVideo(Video video);
        void CreateVideo(Video video);
    }
}
