using Contracts;
using Entities.Exceptions;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    public class EntityChecker
    {
        private readonly IRepositoryManager _repositoryManager;

        public EntityChecker(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<Videostatus> CheckVideoStatusAndGetIfItExist(Guid entityId, bool trackChanges)
        {
            var entity = await _repositoryManager
                .VideoStatuses
                .GetGyConditionAsync(x => x.VideoStatusId == entityId, trackChanges);

            return GenerateNotFoundException(entity, $"Video status с id {entityId} не найден");
        }

        public async Task<User> CheckUserAndGetIfItExist(Guid userId, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserAsync(userId, trackChanges);
            return GenerateNotFoundException(user, $"User с id: {userId} не найден");
        }

        public async Task<User> CheckUserByNicknameAndGetIfItExist(string nickname, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserByNicknameAsync(nickname, trackChanges: false);
            if (user is not null)
                throw new BadRequestException($"Пользователь с nickname {user.Nickname} уже есть");
            return user;
        }

        public async Task<Userrank> CheckUserRankAndGetIfItExist(Guid userRankId, bool trackChanges)
        {
            var userRank = await _repositoryManager.UserRank.GetGyConditionAsync(x => x.UserRankId == userRankId, trackChanges);
            return GenerateNotFoundException(userRank, $"User rank с id: {userRankId} не найден");
        }

        public async Task<Video> CheckVideoAndGetIfItExist(Guid videoId, bool trackChanges)
        {
            var video = await _repositoryManager.Video.GetVideoAsync(videoId, trackChanges);
            return GenerateNotFoundException(video, $"Video с id: {videoId} не найдено");
        }

        public async Task<Videotype> CheckVideoTypeAndGetIfItExist(Guid videoTypeId, bool trackChanges)
        {
            var videoType = await _repositoryManager.VideoType.GetGyConditionAsync(x => x.VideoTypeId == videoTypeId, trackChanges);
            return GenerateNotFoundException(videoType, $"Video type с id {videoTypeId} не найден");
        }

        public async Task<Videorestriction> CheckVideoRestrictionAndGetIfItExist(Guid videoRestrictionId, bool trackChanges)
        {
            var videoRestriction = await _repositoryManager.VideoRestriction.GetGyConditionAsync(x => x.VideoRestrictionId == videoRestrictionId, trackChanges);
            return GenerateNotFoundException(videoRestriction, $"Video с id {videoRestrictionId} не найдено");
        }

        public async Task<Usercomment> CheckUserCommentAndGetIfItExist(Guid videoId,Guid userCommentId, bool trackChanges)
        {
            var userComment = await _repositoryManager.UserComments.GetUserCommentByIdAsync(videoId, userCommentId, trackChanges);
            return GenerateNotFoundException(userComment, $"User comment с id {userCommentId} не найден");
        }

        private TEntity GenerateNotFoundException<TEntity>(TEntity entity, string message) => 
            entity is null
            ? throw new NotFoundException(message)
            : entity;
    }
}
