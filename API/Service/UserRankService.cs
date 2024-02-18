using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Service.Helpers;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserRankService : GenericService<Userrank>, IUserRankService
    {
        private readonly IGenericService<Userrank>.CheckEntityAndGetIfItExist checkEntityAndGetIfItExist;
        public UserRankService(IRepositoryManager repositoryManager, IMapper mapper, EntityChecker entityChecker)
            : base(repositoryManager, mapper)
        {
            checkEntityAndGetIfItExist = entityChecker.CheckUserRankAndGetIfItExist;
        }

        public async Task<ExtentedReferenceDTO> GetUserRankAsync(Guid userRankId, bool trackChanges) => 
            await GetByIdAsync<ExtentedReferenceDTO>(userRankId, trackChanges, checkEntityAndGetIfItExist);

        public async Task<IEnumerable<ExtentedReferenceDTO>> GetUserRanksAsync(bool trackChanges) =>
            await GetAllAsync<ExtentedReferenceDTO>(trackChanges);
    }
}
