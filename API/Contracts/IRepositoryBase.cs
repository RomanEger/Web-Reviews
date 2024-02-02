using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindByConditions(Expression<Func<T, bool>> expression, bool trackChanges);

        void Update(T entity);
        void Delete(T entity);
        void Create(T entity);
    }
}
