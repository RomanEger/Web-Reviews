using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IGenericService<T> where T : class
    {
        Task<IEnumerable<Tentity>> GetAll<Tentity>(bool trackChanges);

        Task<Tentity> GetById<Tentity>(Guid entityId, bool trackChanges);

        Task Update<Tentity>(Guid entityId,Tentity entity, bool trackChanges);
        Task Delete<Tentity>(Guid entityId, bool trackChanges);
        Task<T> Create<Tentity>(Tentity entity);   
    }
}
