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

        Task<T> UpdateAsync<Tentity>(Guid entityId,Tentity entityForManipulation, bool trackChanges);
        Task DeleteAsync(Guid entityId, bool trackChanges);
        Task<T> CreateAsync<Tentity>(Tentity entity);   
    }
}
