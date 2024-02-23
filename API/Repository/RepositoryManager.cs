using Contracts;
using Entities.Exceptions;
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
        private readonly Lazy<IGenericRepository<Videotype>> _videoTypeRepository;
        private readonly Lazy<IGenericRepository<Videorestriction>> _videoRestrictionRepository;
        private readonly Lazy<IGenericRepository<Userrank>> _userRankRepository;
        private readonly Lazy<IGenericRepository<Videogenre>> _videoGenreRepository;
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<IVideoRepository> _videoRepository;

        public RepositoryManager(WebReviewsContext webReviewsContext)
        {
            _webReviewsContext = webReviewsContext;
            _videoStatusesRepository = new Lazy<IGenericRepository<Videostatus>>(() => new GenericRepository<Videostatus>(webReviewsContext));
            _videoTypeRepository = new Lazy<IGenericRepository<Videotype>>(() => new GenericRepository<Videotype>(webReviewsContext));
            _videoRestrictionRepository = new Lazy<IGenericRepository<Videorestriction>>(() => new GenericRepository<Videorestriction>(webReviewsContext));
            _userRankRepository = new Lazy<IGenericRepository<Userrank>>(() => new GenericRepository<Userrank>(webReviewsContext));
            _videoGenreRepository = new Lazy<IGenericRepository<Videogenre>>(() => new GenericRepository<Videogenre>(webReviewsContext));
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(webReviewsContext));
            _videoRepository = new Lazy<IVideoRepository>(() => new VideoRepository(webReviewsContext));
        }

        public IGenericRepository<Videostatus> VideoStatuses => _videoStatusesRepository.Value;

        public IUserRepository User => _userRepository.Value;

        public IGenericRepository<Userrank> UserRank => _userRankRepository.Value;

        public IVideoRepository Video => _videoRepository.Value;

        public IGenericRepository<Videotype> VideoType => _videoTypeRepository.Value;

        public IGenericRepository<Videorestriction> VideoRestriction => _videoRestrictionRepository.Value;

        public IGenericRepository<Videogenre> VideoGenre => _videoGenreRepository.Value;

        public async Task SaveAsync() =>        
            await _webReviewsContext.SaveChangesAsync();

        public IGenericRepository<T> Set<T>() where T : class
        {
            //if (typeof(T).IsAssignableFrom(typeof(Videostatus)))
            //   return (IGenericRepository<T>)VideoStatuses;

            //var type = typeof(T).GetType();
            var genericRepository = true switch
            {
                true when typeof(T).IsAssignableFrom(typeof(Videostatus)) => (IGenericRepository<T>)VideoStatuses,
                true when typeof(T).IsAssignableFrom(typeof(Userrank)) => (IGenericRepository<T>)UserRank,
                _ => throw new NotFoundException("Type are incorrect")
            };

            return genericRepository;
            //return (IGenericRepository<T>)VideoStatuses;
        }
    }
}
