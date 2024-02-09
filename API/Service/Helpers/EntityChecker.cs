﻿using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Repository;
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
                .Set<Videostatus>()
                .GetGyConditionAsync(x => x.VideoStatusId == entityId, trackChanges);

            return GenerateNotFoundException(entity, $"Entity {typeof(Videostatus).Name} with id {entityId} not found");
        }

        public async Task<User> CheckUserAndGetIfItExist(Guid userId, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserAsync(userId, trackChanges);
            return GenerateNotFoundException(user, $"User with id: {userId} was not found");
        }

        public async Task<User> GetUserByNicknameAndCheck(string nickname, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserByNicknameAsync(nickname, trackChanges: false);
            if (user is not null)
                throw new BadRequestException($"User with nickname {user.Nickname} already exist");
            return user;
        }

        public async Task<Userrank> GetUserRankAndCheckIfItExist(Guid userRankId, bool trackChanges)
        {
            var userRank = await _repositoryManager.UserRank.GetGyConditionAsync(x => x.UserRankId == userRankId, trackChanges);
            return GenerateNotFoundException(userRank, $"User rank with id: {userRankId} was not found");
        }

        private TEntity GenerateNotFoundException<TEntity>(TEntity entity, string message) => 
            entity is null
            ? throw new NotFoundException(message)
            : entity;
    }
}