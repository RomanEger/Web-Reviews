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
        public UserService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<UserDTO> CreateUserAsync(UserForRegistrationDTO userForRegistration)
        {
            var user = await _repositoryManager.User.GetUserByEmailAsync(userForRegistration.Nickname, trackChanges: false);
            if (user is not null)
                throw new BadRequestException($"User with email {user.Email} already exist");

            await GetUserByNicknameAndCheck(userForRegistration.Nickname, trackChanges: false);
            
            userForRegistration.Password = PasswordHash.EncodePasswordToBase64(userForRegistration.Password);

            user = _mapper.Map<User>(userForRegistration);

            _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(user);
            return userToReturn;
        }

        public async Task DeleteUserAsync(Guid userId, bool trackChanges)
        {
            var user = await CheckUserAndGetIfItExist(userId, trackChanges);
            _repositoryManager.User.DeleteUser(user);
            await _repositoryManager.SaveAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid userId, bool trackChanges)
        {
            var user = await CheckUserAndGetIfItExist(userId, trackChanges);
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
            await GetUserByNicknameAndCheck(userForUpdate.Nickname, trackChanges);
            var user = await CheckUserAndGetIfItExist(userId, trackChanges);

            if(userForUpdate.Password != user.Password)
                userForUpdate.Password = PasswordHash.EncodePasswordToBase64(userForUpdate.Password);
            
            _mapper.Map(userForUpdate, user);
            await _repositoryManager.SaveAsync();

            var userToReturn = _mapper.Map<UserDTO>(user);
            return userToReturn;
        }

        private async Task<User> CheckUserAndGetIfItExist(Guid userId, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserAsync(userId, trackChanges);
            if (user is null)
                throw new NotFoundException($"User with id: {userId} was not found");
            return user;
        }

        private async Task<User> GetUserByNicknameAndCheck(string nickname, bool trackChanges)
        {
            var user = await _repositoryManager.User.GetUserByNicknameAsync(nickname, trackChanges: false);
            if (user is not null)
                throw new BadRequestException($"User with nickname {user.Nickname} already exist");
            return user;
        }
    }
}
