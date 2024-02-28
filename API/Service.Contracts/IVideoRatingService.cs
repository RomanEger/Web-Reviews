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
        Task<VideoDTO> UpdateVideoRating(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges);
        Task<VideoDTO> CreateVideoRating(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges);
        Task<VideoDTO> RefreshVideoRating(Guid videoId, bool trackChanges);
    }
}
