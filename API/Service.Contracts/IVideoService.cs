using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IVideoService
    {
        Task<(IEnumerable<VideoDTO> videos, MetaData metaData)> GetVideosAsync(VideoParameters videoParameters, bool trackChanges);
        Task<VideoDTO> GetVideoByIdAsync(Guid videoId, bool trackChanges);
        Task<VideoDTO> CreateVideoAsync(VideoForManipulationDTO videoForManipulationDTO);
        Task DeleteVideoAsync(Guid videoId, bool trackChanges);
        Task<VideoDTO> UpdateVideoAsync(Guid videoId, VideoForManipulationDTO videoForManipulation, bool trackChanges);
    }
}
