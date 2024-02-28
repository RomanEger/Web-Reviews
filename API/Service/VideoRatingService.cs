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

        public async Task<VideoDTO> CreateVideoRating(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges)
        {
            await _entityChecker.CheckUserAndGetIfItExist(ratingForManipulationDTO.UserId, trackChanges: false);
            await _entityChecker.CheckVideoAndGetIfItExist(ratingForManipulationDTO.VideoId, trackChanges: false);

            var videoRating = await _repositoryManager.VideoRating.GetVideoRatingAsync(ratingForManipulationDTO.VideoId,
                                                                                       ratingForManipulationDTO.UserId,
                                                                                       trackChanges);
            if (videoRating is null)
            {
                videoRating = _mapper.Map<Videorating>(ratingForManipulationDTO);
                _repositoryManager.VideoRating.CreateVideoRating(videoRating);
            }
            else
                _mapper.Map(ratingForManipulationDTO, videoRating);

            await _repositoryManager.SaveAsync();

            return await RefreshVideoRating(ratingForManipulationDTO.VideoId, trackChanges);
        }

        public async Task<VideoDTO> RefreshVideoRating(Guid videoId, bool trackChanges)
        {
            var video = await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges);
            var videoRatings = await _repositoryManager.VideoRating.GetVideoRatingsAsync(video.VideoId, trackChanges: false);
            var averageRating = videoRatings.Average(x => x.Rating);
            video.Rating = (decimal)averageRating;

            await _repositoryManager.SaveAsync();
            var videoToReturn = _mapper.Map<VideoDTO>(video);
            return videoToReturn;
        }

        public async Task<VideoDTO> UpdateVideoRating(VideoRatingForManipulationDTO ratingForManipulationDTO, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
