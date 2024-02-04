using AutoMapper;
using Entities.Exceptions;
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

        public async Task<T> Create<Tentity>(Tentity entity)
        {
            var mapEntity = _mapper.Map<T>(entity);

            if (mapEntity is null)
                throw new BadRequestException($"This entity {entity.GetType().Name} is incorrect");

            _repositoryManager.Set<T>().CreateEntity(mapEntity);
            await _repositoryManager.SaveAsync();

            return mapEntity;
        }            

        public Task Delete<Tentity>(Guid entityId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tentity>> GetAll<Tentity>(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<Tentity> GetById<Tentity>(Guid entityId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task Update<Tentity>(Guid entityId, Tentity entity, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
