using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WebReviewsContext _webReviewsContext;
        private readonly Lazy<IGenericRepository<Videostatus>> _videoStatusesRepository;

        public RepositoryManager(WebReviewsContext webReviewsContext)
        {
            _webReviewsContext = webReviewsContext;
            _videoStatusesRepository = new Lazy<IGenericRepository<Videostatus>>(() => new GenericRepository<Videostatus>(webReviewsContext));
        }

        public IGenericRepository<Videostatus> VideoStatuses => _videoStatusesRepository.Value;

        public async Task SaveAsync() =>        
            await _webReviewsContext.SaveChangesAsync();

        public IGenericRepository<T> Set<T>() where T : class
        {
            //if (typeof(T).IsAssignableFrom(typeof(Videostatus)))
            //    return (IGenericRepository<T>)VideoStatuses;
            return (IGenericRepository<T>)VideoStatuses;
        }
    }
}
