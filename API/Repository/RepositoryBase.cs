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
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        protected WebReviewsContext webReviewsContext;

        protected RepositoryBase(WebReviewsContext webReviewsContext)
        {
            this.webReviewsContext = webReviewsContext;
        }

        public void Create(T entity) => 
            webReviewsContext.Set<T>().Add(entity);    

        public void Delete(T entity) =>
            webReviewsContext.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) =>
            trackChanges
            ? webReviewsContext.Set<T>()
            : webReviewsContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByConditions(Expression<Func<T, bool>> expression, bool trackChanges) =>
            trackChanges
            ? webReviewsContext.Set<T>().Where(expression)
            : webReviewsContext.Set<T>().Where(expression).AsNoTracking();


        public void Update(T entity) =>
            webReviewsContext.Set<T>().Update(entity);
    }
}
