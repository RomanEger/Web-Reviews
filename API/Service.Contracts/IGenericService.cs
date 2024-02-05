using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<Tentity>> GetAllAsync<Tentity>(bool trackChanges);

        Task<Tentity> GetByIdAsync<Tentity>(Guid entityId, bool trackChanges);

        Task<TentityToReturn> UpdateAsync<TentityToChange, TentityToReturn>(Guid entityId, TentityToChange entityForManipulation, bool trackChanges);
        Task DeleteAsync(Guid entityId, bool trackChanges);
        Task<T> CreateAsync<Tentity>(Tentity entity);   
    }
}
