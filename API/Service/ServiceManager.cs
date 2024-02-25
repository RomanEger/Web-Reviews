using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Microsoft.Extensions.Options;
using Repository;
using Service.Contracts;
using Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IVideoStatusesService> _videoStatusesService;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IVideoService> _videoService;
        private readonly Lazy<IUserRankService> _userRankService;
        private readonly Lazy<IUserCommentService> _userCommentService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker, IOptions<JwtConfiguration> configuration)
        {
            _videoStatusesService = new Lazy<IVideoStatusesService>(() => new VideoStatusesService(repositoryManager, mapper, entityChecker));
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, entityChecker));
            _videoService = new Lazy<IVideoService>(() => new VideoService(repositoryManager, mapper, entityChecker));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, mapper, configuration, entityChecker));
            _userRankService = new Lazy<IUserRankService>(() => new UserRankService(repositoryManager, mapper, entityChecker));
            _userCommentService = new Lazy<IUserCommentService>(() => new UserCommentService(repositoryManager, mapper, entityChecker));
        }

        public IVideoStatusesService VideoStatuses => _videoStatusesService.Value;

        public IUserService User => _userService.Value;

        public IAuthenticationService Authentication => _authenticationService.Value;

        public IUserRankService UserRank => _userRankService.Value;

        public IVideoService Video => _videoService.Value;

        public IUserCommentService UserComment => _userCommentService.Value;
    }
}
