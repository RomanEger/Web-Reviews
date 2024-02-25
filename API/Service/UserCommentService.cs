using AutoMapper;
using Contracts;
using Entities.Models;
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
    public class UserCommentService : IUserCommentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly EntityChecker _entityChecker;

        public UserCommentService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _entityChecker = entityChecker;
        }

        public async Task<UserCommentDTO> CreateUserCommentAsync(Guid videoId, UserCommentForManipulationDTO commentForManipulation)
        {
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);
            await _entityChecker.CheckUserAndGetIfItExist(commentForManipulation.UserId, trackChanges: false);
            var userComment = _mapper.Map<Usercomment>(commentForManipulation);

            userComment.VideoId = videoId;
            _repositoryManager.UserComments.CreateUserComment(userComment);
            await _repositoryManager.SaveAsync();

            var userCommentToReturn = _mapper.Map<UserCommentDTO>(userComment);
            return userCommentToReturn;
        }

        public async Task DeleteUserCommentAsync(Guid videoId, Guid commentId, bool trackChanges)
        {
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);
            var userComment = await _entityChecker.CheckUserCommentAndGetIfItExist(videoId, commentId, trackChanges);

            _repositoryManager.UserComments.DeleteUserComment(userComment);
            await _repositoryManager.SaveAsync();
        }

        public async Task<UserCommentDTO> GetUserCommentById(Guid videoId, Guid commentId, bool trackChanges)
        {
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);
            var userComment = await _entityChecker.CheckUserCommentAndGetIfItExist(videoId, commentId, trackChanges);

            var userCommentToReturn = _mapper.Map<UserCommentDTO>(userComment);
            return userCommentToReturn;
        }

        public async Task<(IEnumerable<UserCommentDTO> comments, MetaData metaData)> GetUserCommentsAsync(
            Guid videoId, UserCommentsParameters commentsParameters, bool trackChanges)
        {
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);
            var userComments = await _repositoryManager.UserComments.GetUserCommentsAsync(videoId, commentsParameters, trackChanges);

            var userCommentToReturn = _mapper.Map<IEnumerable<UserCommentDTO>>(userComments);
            return (userCommentToReturn, userComments.MetaData);
        }

        public async Task<UserCommentDTO> UpdateUserCommentAsync(Guid videoId, Guid commentId, UserCommentForManipulationDTO commentForManipulation, bool trackChanges)
        {
            await _entityChecker.CheckVideoAndGetIfItExist(videoId, trackChanges: false);
            await _entityChecker.CheckUserAndGetIfItExist(commentForManipulation.UserId, trackChanges: false);
            var userComment = await _entityChecker.CheckUserCommentAndGetIfItExist(videoId, commentId, trackChanges);

            _mapper.Map(commentForManipulation, userComment);
            await _repositoryManager.SaveAsync();

            var userCommentToReturn = _mapper.Map<UserCommentDTO>(userComment);
            return userCommentToReturn;
        }
    }
}
