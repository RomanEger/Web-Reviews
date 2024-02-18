using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VideoService : IVideoService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly EntityChecker _entityChecker;

        public VideoService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _entityChecker = entityChecker;
        }

        public async Task<VideoDTO> CreateVideoAsync(VideoForManipulationDTO videoForManipulationDTO)
        {
            await CheckReferences(videoForManipulationDTO);

            var video = _mapper.Map<Video>(videoForManipulationDTO);
            _repositoryManager.Video.CreateVideo(video);
            await _repositoryManager.SaveAsync();

            var videoToReturn = _mapper.Map<VideoDTO>(video);
            return videoToReturn;
        }

        public async Task DeleteVideoAsync(Guid videoId, bool trackChanges)
        {
            var video = await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges);
            _repositoryManager.Video.DeleteVideo(video);
            await _repositoryManager.SaveAsync();
        }

        public async Task<VideoDTO> GetVideoByIdAsync(Guid videoId, bool trackChanges)
        {
            var video = await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges);
            var videoToReturn = _mapper.Map<VideoDTO>(video);
            return videoToReturn;
        }

        public async Task<(IEnumerable<VideoDTO> videos, MetaData metaData)> GetVideosAsync(VideoParameters videoParameters, bool trackChanges)
        {
            //if(videoParameters.GenreIds is null)
            var videoFiltertedGenre = await _repositoryManager.VideoGenre
                .GetAllByConditionAsync(x => videoParameters.GenreIds.Count(v => v == x.GenreId) == videoParameters.GenreIds.Count, trackChanges);

            videoFiltertedGenre.DistinctBy(x => x.VideoId);

            var videos = await _repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges);
            var sortedVideos = videos.Where(x => videoFiltertedGenre.Any(v => v.VideoId == x.VideoId));

            var videosToReturn = _mapper.Map<IEnumerable<VideoDTO>>(sortedVideos);
            return (videos: videosToReturn, metaData: videos.MetaData);
        }

        public async Task<VideoDTO> UpdateVideoAsync(Guid videoId, VideoForManipulationDTO videoForManipulation, bool trackChanges)
        {
            await CheckReferences(videoForManipulation);

            var video = await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges);
            _mapper.Map(videoForManipulation, video);
            await _repositoryManager.SaveAsync();

            var videoToReturn = _mapper.Map<VideoDTO>(video);
            return videoToReturn;
        }

        private async Task CheckReferences(VideoForManipulationDTO videoForManipulationDTO)
        {
            await _entityChecker.CheckVideoStatusAndGetIfItExist(videoForManipulationDTO.VideoStatusId, trackChanges: false);
            await _entityChecker.CheckVideoTypeAndGetIfItExist(videoForManipulationDTO.VideoTypeId, trackChanges: false);
            await _entityChecker.CheckVideoRestrictionAndGetIfItExist(videoForManipulationDTO.VideoRestrictionId, trackChanges: false);
        }
    }
}
