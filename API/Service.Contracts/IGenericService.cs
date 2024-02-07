using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace Service.Contracts
{
    public interface IGenericService<T> where T : class
    {
        delegate Task<T> CheckEntityAndGetIfItExist(Guid entityId, bool trackChanges);
        Task<IEnumerable<Tentity>> GetAllAsync<Tentity>(bool trackChanges);

        Task<Tentity> GetByIdAsync<Tentity>(Guid entityId, bool trackChanges, CheckEntityAndGetIfItExist getIfItExist);

        Task<TentityToReturn> UpdateAsync<TentityToChange, TentityToReturn>(Guid entityId,
                                                                            TentityToChange entityForManipulation,
                                                                            bool trackChanges,
                                                                            IGenericService<T>.CheckEntityAndGetIfItExist getEntityIfItExist);
        Task DeleteAsync(Guid entityId, bool trackChanges, IGenericService<T>.CheckEntityAndGetIfItExist getEntityIfItExist);
        Task<TentityToReturn> CreateAsync<TentityToChange, TentityToReturn>(TentityToChange entity);
    }
}
