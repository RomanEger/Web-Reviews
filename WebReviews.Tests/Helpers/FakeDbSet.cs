using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebReviews.Tests.Helpers
{
    public class FakeDbSet<T> : DbSet<T>, IDbSet<T>,IEnumerable<T>, IQueryable<T>, IDbAsyncEnumerable<T> where T : class
    {
        private ObservableCollection<T> _data;
        IQueryable<T> _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public Type ElementType { get { return _query.ElementType; } }

        public Expression Expression { get { return _query.Expression; } }

        ObservableCollection<T> IDbSet<T>.Local
        {
            get { return _data; }
        }

        IQueryProvider IQueryable.Provider { get { return _query.Provider; } }

        public override T Add(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override T Attach(T entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public override T Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override T Remove(T entity)
        {
            throw new NotImplementedException();
        }

        TDerivedEntity IDbSet<T>.Create<TDerivedEntity>()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            return base.AddRange(entities);
        }
    }
}
