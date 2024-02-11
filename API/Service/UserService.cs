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
    public class UserService :  IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly EntityChecker _entityChecker;
        public UserService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _entityChecker = entityChecker;
        }

       

        public async Task DeleteUserAsync(Guid userId, bool trackChanges)
        {
            var user = await _entityChecker.CheckUserAndGetIfItExist(userId, trackChanges);
            _repositoryManager.User.DeleteUser(user);
            await _repositoryManager.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid userId, bool trackChanges)
        {
            var user = await _entityChecker.CheckUserAndGetIfItExist(userId, trackChanges);
            var userToReturn = _mapper.Map<UserDTO>(user);
            return userToReturn;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync(bool trackChanges)
        {
            var users = await _repositoryManager.User.GetUsersAsync(trackChanges);
            var usersToReturn = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersToReturn;
        }

        //Проверка на user rank id
        public async Task<UserDTO> UpdateUserAsync(Guid userId, UserForUpdateDTO userForUpdate, bool trackChanges)
        {
            await _entityChecker.GetUserByNicknameAndCheck(userForUpdate.Nickname, trackChanges);
            await _entityChecker.GetUserRankAndCheckIfItExist((Guid)userForUpdate.UserRankId, trackChanges: false);
            var user = await _entityChecker.CheckUserAndGetIfItExist(userId, trackChanges);

            if(userForUpdate.Password != user.Password)
                userForUpdate.Password = PasswordHash.EncodePasswordToBase64(userForUpdate.Password);
            
            _mapper.Map(userForUpdate, user);
            await _repositoryManager.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(user);
            return userToReturn;
        }        
    }
}
