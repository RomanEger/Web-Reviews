using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IVideoRatingService
    {
        Task<VideoDTO> CreateOrUpdateVideoRatingAsync(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges);
        Task<VideoDTO> RefreshVideoRatingAsync(Guid videoId, bool trackChanges);
        Task<VideoRatingDTO> GetUserVideoRatingAsync(Guid videoId, Guid userId, bool trackChanges);
    }
}
