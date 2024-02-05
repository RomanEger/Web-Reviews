using AutoMapper;
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

        public ServiceManager(RepositoryManager repositoryManager, IMapper mapper)
        {
            _videoStatusesService = new Lazy<IVideoStatusesService>(() => new VideoStatusesService(repositoryManager, mapper));
        }

        public IVideoStatusesService VideoStatuses => _videoStatusesService.Value;
    }
}
