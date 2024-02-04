using AutoMapper;
using Entities.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VideoStatusesService : GenericService<Videostatus>
    {
        public VideoStatusesService(RepositoryManager repositoryManager, IMapper mapper)
            : base(repositoryManager, mapper)
        {
        }

        
    }
}
