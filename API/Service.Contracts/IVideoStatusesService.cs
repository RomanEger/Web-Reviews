using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IVideoStatusesService
    {
        Task<ReferenceDTO> CreateVideoStatusAsync(ReferenceForManipulationDTO manipulationDTO);
        Task DeleteVideoStatusAsync(Guid videoStatusId, bool trackChanges);
        Task<IEnumerable<ReferenceDTO>> GetVideoStatusesAsync(bool trackChanges);
        Task<ReferenceDTO> GetVideoStatusByIdAsync(Guid videoStatusId,bool trackChanges);
        Task<ReferenceDTO> UpdateVideoStatus(Guid videoStatusId, ReferenceForManipulationDTO manipulationDTO, bool trackChanges);
    }
}
