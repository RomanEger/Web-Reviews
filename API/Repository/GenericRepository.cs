using Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository<T> : RepositoryBase<T>, IGenericRepository<T> where T : class
    {
        public GenericRepository(WebReviewsContext webReviewsContext)
            : base(webReviewsContext)
        {
        }

        public void CreateEntity(T entity) =>
            Create(entity);

        public void DeleteEntity(T entity) =>
            Delete(entity);

        public async Task<IEnumerable<T>> GetAllAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .ToListAsync();

        public async Task<T> GetGyConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) =>
            await FindByConditions(expression, trackChanges)
            .SingleOrDefaultAsync();            

        public void UpdateEntity(T entity) =>
            Update(entity);
    }
}
