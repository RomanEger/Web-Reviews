using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VideoRatingService : IVideoRatingService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly EntityChecker _entityChecker;

        public VideoRatingService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _entityChecker = entityChecker;
        }

        public async Task<VideoDTO> CreateOrUpdateVideoRatingAsync(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges)
        {
            var videoRating = await CheckDependenciesAndGetVideoRating(ratingForManipulationDTO.VideoId,
                                                                       ratingForManipulationDTO.UserId,
                                                                       trackChanges: false);
            if (videoRating is null)
            {
                videoRating = _mapper.Map<Videorating>(ratingForManipulationDTO);
                _repositoryManager.VideoRating.CreateVideoRating(videoRating);
            }
            else
                _mapper.Map(ratingForManipulationDTO, videoRating);

            await _repositoryManager.SaveAsync();

            return await RefreshVideoRatingAsync(ratingForManipulationDTO.VideoId, trackChanges);
        }

        private async Task<Videorating?> CheckDependenciesAndGetVideoRating(Guid videoId, Guid userId, bool trackChanges)
        {
            await _entityChecker.CheckUserAndGetIfItExist(userId, trackChanges: false);
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);

            return await _repositoryManager.VideoRating.GetVideoRatingAsync(videoId, userId ,trackChanges);
        }

        public async Task<VideoRatingDTO> GetUserVideoRatingAsync(Guid videoId, Guid userId, bool trackChanges)
        {
            var videoRating = await CheckDependenciesAndGetVideoRating(videoId, userId, trackChanges);
            var videoRatingDTO = _mapper.Map<VideoRatingDTO>(videoRating);
            return videoRatingDTO;
        }

        public async Task<VideoDTO> RefreshVideoRatingAsync(Guid videoId, bool trackChanges)
        {
            var video = await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges);
            var videoRatings = await _repositoryManager.VideoRating.GetVideoRatingsAsync(video.VideoId, trackChanges: false);
            var averageRating = videoRatings.Average(x => x.Rating);
            video.Rating = (decimal)averageRating;

            await _repositoryManager.SaveAsync();
            var videoToReturn = _mapper.Map<VideoDTO>(video);
            return videoToReturn;
        }

        public async Task<IEnumerable<VideoRatingDTO>> GetUserRatingsAsync(Guid userId, bool trackChanges)
        {
            var user = await _entityChecker.CheckUserAndGetIfItExist(userId, trackChanges);
            var ratings = await _repositoryManager.VideoRating.GetUserRatingsAsync(user.UserId, false);
            var ratingsToReturn = _mapper.Map<IEnumerable<VideoRatingDTO>>(ratings);
            return ratingsToReturn;
        }
    }
}
