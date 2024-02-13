using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IUserRankService
    {
        Task<IEnumerable<ExtentedReferenceDTO>> GetUserRanksAsync(bool trackChanges);
        Task<ExtentedReferenceDTO> GetUserRankAsync(Guid userRankId, bool trackChanges);
    }
}
