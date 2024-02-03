using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges);
        Task<T> GetGyConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges);
        void DeleteEntity(T entity);
        void CreateEntity(T entity);
        void UpdateEntity(T entity);
    }
}
