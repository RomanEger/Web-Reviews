using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Repository;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        protected RepositoryManager _repositoryManager;
        protected IMapper _mapper;

        protected GenericService(RepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<T> CreateAsync<Tentity>(Tentity entity)
        {
            if (entity is null)
                throw new BadRequestException($"This entity {typeof(Tentity).Name} is incorrect");

            var mapEntity = _mapper.Map<T>(entity);
            
            _repositoryManager.Set<T>().CreateEntity(mapEntity);
            await _repositoryManager.SaveAsync();

            return mapEntity;
        }            


        public async Task DeleteAsync(Guid entityId, bool trackChanges)
        {
            var entity = await _repositoryManager
                .Set<T>()
                .GetGyConditionAsync(x => (Guid)x.GetType().GetProperties().First().GetValue(x) == entityId, trackChanges);

            if (entity is null)
                throw new NotFoundException($"entity {typeof(T).Name} with Id {entityId} not found");

            _repositoryManager.Set<T>().DeleteEntity(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync<Tentity>(bool trackChanges)
        {
            var entities = await _repositoryManager.Set<T>().GetAllAsync(trackChanges);
            var entitiesToReturn = _mapper.Map<IEnumerable<Tentity>>(entities);
            return entitiesToReturn;
        }

        public async Task<Tentity> GetByIdAsync<Tentity>(Guid entityId, bool trackChanges)
        {
            var entity = await _repositoryManager
                .Set<T>()
                .GetGyConditionAsync(x => (Guid)x.GetType().GetProperties().First().GetValue(x) == entityId, trackChanges);

            if (entity is null)
                throw new NotFoundException($"Entity {typeof(T).Name} with id {entityId} not found");

            var entityToReturn = _mapper.Map<Tentity>(entity);
            return entityToReturn;
        }

        public async Task<TentityToReturn> UpdateAsync<TentityToChange, TentityToReturn>(Guid entityId, TentityToChange entityForManipulation, bool trackChanges)
        {
            var entity = await _repositoryManager
                .Set<T>()
                .GetGyConditionAsync(x => (Guid)x.GetType().GetProperties().First().GetValue(x) == entityId, trackChanges);

            if (entity is null)
                throw new NotFoundException($"Entity {typeof(T).Name} with id {entityId} not found");

            _mapper.Map(entityForManipulation, entity);
            await _repositoryManager.SaveAsync();

            var entityToReturn = _mapper.Map<TentityToReturn>(entity);
            return entityToReturn;
        }
    }
}
