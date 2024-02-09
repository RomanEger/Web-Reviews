using AutoMapper;
using Contracts;
using Repository;
using Service.Contracts;
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

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _videoStatusesService = new Lazy<IVideoStatusesService>(() => new VideoStatusesService(repositoryManager, mapper));
            _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper));
        }

        public IVideoStatusesService VideoStatuses => _videoStatusesService.Value;

        public IUserService User => _userService.Value;
    }
}
