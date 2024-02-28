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
        Task<VideoDTO> CreateOrUpdateVideoRating(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges);
        Task<VideoDTO> RefreshVideoRating(Guid videoId, bool trackChanges);
    }
}
