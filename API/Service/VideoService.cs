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
            var videos = await _repositoryManager.Video.GetVideosAsync(videoParameters, trackChanges);
            if (videoParameters.GenreIds is not null)
            {
                // Выборка из таблицы VideoGenre с условием, что жанр должен находиться в параметрах фильтрации
                var videoGenre = await _repositoryManager.VideoGenre
                    .GetAllByConditionAsync(x => videoParameters.GenreIds.Contains(x.GenreId), trackChanges);

                // Группировка по videoID для формирования нового списка в котором будет список жанров конкректного видео
                var sortedVideoGenres = from x in videoGenre
                           group x by x.VideoId into g
                           select new
                           {
                               VideoId = g.Key,
                               GenreIds = from genre in g select genre.GenreId
                           };

                // Фильтрации видео у которых подходят не все жанры
                sortedVideoGenres = sortedVideoGenres.Where(x => x.GenreIds.Any(videoParameters.GenreIds.Contains)
                                                                 && videoParameters.GenreIds.Count() == x.GenreIds.Count());

                // Соединения отфильтрованных видео изначально и видео отфильтрованных по жанрам
                videos = PagedList<Video>.ToPageList(videos.Where(x => sortedVideoGenres.Any(v => v.VideoId == x.VideoId)), videoParameters);
            }

            var videosToReturn = _mapper.Map<IEnumerable<VideoDTO>>(videos);
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
